namespace back.Application.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using DTOs;
using Common;
using Interfaces;


public class ResumeAnalysisService : IResumeAnalysisService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ResumeAnalysisService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Result<List<SuggestionDto>>> AnalyzeResumeAsync(ResumeDtoForAi resume)
        {
            try
            {
                // Получаем параметры из конфигурации
                var authUrl = _configuration["AI:AuthUrl"]; // например, "https://ngw.devices.sberbank.ru:9443/api/v2/oauth"
                var gigachatUrl = _configuration["AI:GigaChatApiUrl"]; // например, "https://gigachat.devices.sberbank.ru/api/v1/chat/completions"
                var authorizationKey = _configuration["AI:AuthorizationKey"]; // ваш ключ
                var scope = _configuration["AI:Scope"]; // например, "GIGACHAT_API_PERS"

                var token = await GetAccessTokenAsync(authUrl, authorizationKey, scope);
                if (string.IsNullOrEmpty(token))
                    return Result<List<SuggestionDto>>.Failure("Access token not obtained");

                // Системный промпт (копируйте полный текст, как в вашем скрипте)
                string systemPrompt = @"
Ты — профессиональный AI-движок по улучшению резюме.

Ты получаешь резюме в формате JSON. Ты должен вернуть массив объектов-предложений строго в следующем формате:

[
  {
    ""id"": ""строка (sug-{уникальныйId})"",
    ""type"": ""experience | skills | description | etc"",
    ""title"": ""строка"",
    ""description"": ""строка"",
    ""confidence"": число (от 0.0 до 1.0),
    ""before"": ""строка или массив строк"",
    ""after"": ""строка или массив строк"",
    ""reasoning"": ""строка""
  }
]

🛑 ВАЖНЫЕ ПРАВИЛА (СТРОГО):
- НЕ предлагай улучшения для образования — полностью пропускай.
- Если type === ""skills"":
  - ""before"" и ""after"" должны быть массивами строк.
  - Добавляй только реалистичные новые навыки.
- Если type === ""experience"":
  - ""before"" — обычная строка (текущее описание работы),
  - ""after"" должно быть: ""{целеваяДолжность} + твоя улучшенная версия"".
  - Текст должен быть одной строкой, не списком.
- Если type === ""description"":
  - Те же правила, что и для ""experience"": строка → строка с префиксом {целеваяДолжность}.
- Никогда не используй шаблоны типа ""{company}"" или ""{description}"" — только реальные данные.
- Сохраняй форматирование: если ""before"" был списком, ""after"" тоже должен быть списком.
- Не используй Markdown или комментарии — только чистый JSON.

Результат ДОЛЖЕН быть строго валидным JSON.
".Trim();

                // Формируем payload
                var payload = new
                {
                    model = "GigaChat",
                    stream = false,
                    update_interval = 0,
                    messages = new object[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = JsonConvert.SerializeObject(resume, Formatting.None) }
                    }
                };

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, gigachatUrl);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var payloadJson = JsonConvert.SerializeObject(payload);
                requestMessage.Content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                // Парсинг ответа
                dynamic responseJson = JsonConvert.DeserializeObject(responseContent);
                string messageContent = responseJson.choices[0]?.message?.content;
                if (string.IsNullOrWhiteSpace(messageContent))
                    return Result<List<SuggestionDto>>.Failure("No content in AI response");

                // Удаляем Markdown-обёртку (например, ```json ... ```)
                messageContent = Regex.Replace(messageContent.Trim(), @"^```json\s*|\s*```$", "", RegexOptions.Singleline);

                var suggestions = JsonConvert.DeserializeObject<List<SuggestionDto>>(messageContent);
                if (suggestions == null)
                    return Result<List<SuggestionDto>>.Failure("Failed to parse suggestions");

                return Result<List<SuggestionDto>>.Success(suggestions);
            }
            catch (Exception ex)
            {
                return Result<List<SuggestionDto>>.Failure($"Error during resume analysis: {ex.Message}");
            }
        }

        private async Task<string> GetAccessTokenAsync(string authUrl, string authorizationKey, string scope)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, authUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("RqUID", Guid.NewGuid().ToString());
            request.Headers.Add("Authorization", $"Basic {authorizationKey}");
            var contentData = new Dictionary<string, string> { { "scope", scope } };
            request.Content = new FormUrlEncodedContent(contentData);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            dynamic obj = JsonConvert.DeserializeObject(json);
            return obj.access_token;
        }
    }
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
        string CleanString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;
            string unescaped = Regex.Unescape(input);
            return unescaped.Replace("\n", "").Replace("\r", "").Trim();
        }

        try
        {
            var authUrl = _configuration["AI:AuthUrl"];
            var gigachatUrl = _configuration["AI:GigaChatApiUrl"];
            var authorizationKey = _configuration["AI:AuthorizationKey"];
            var scope = _configuration["AI:Scope"];

            var token = await GetAccessTokenAsync(authUrl, authorizationKey, scope);
            if (string.IsNullOrEmpty(token))
                return Result<List<SuggestionDto>>.Failure("Access token not obtained");

            string systemPrompt = @"
Ты — профессиональный AI-движок по улучшению резюме. Твоя задача — проанализировать входное резюме, представленное в формате JSON, и вернуть массив объектов-предложений. Каждый объект должен описывать конкретное улучшение резюме и соответствовать строго следующему формату:

Каждый объект предложения должен иметь следующие поля:
- ""id"": строка, вида ""sug-{номер}"" (например, ""sug-1"", ""sug-2"", …). Это уникальный идентификатор предложения.
- ""type"": один из вариантов: ""title"", ""job"", ""skills"", ""experience"" или ""description"". Он указывает, к какому полю резюме относится предложение.
- ""title"": краткое, но конкретное название предложения. Не используй шаблонные фразы вроде ""Усовершенствованное резюме"". Например: ""Уточнение специализации в заголовке"".
- ""description"": подробное описание предлагаемых изменений. Здесь должно быть объяснено, что именно и почему предлагается изменить.
- ""confidence"": число от 0.0 до 1.0, показывающее уверенность ИИ в том, что предложение улучшит резюме.
- ""before"": исходное значение изменяемого поля, которое нужно сохранить. Если поле является строкой (например, ""title"", ""job"" или ""description""), то ""before"" — это строка. Если поле — массив (например, ""skills""), то ""before"" должен быть массивом строк. Если поле ""experience"", то ""before"" должен быть массивом объектов, где каждый объект соответствует записи об опыте и включает поля ""company"", ""position"", ""startDate"", ""endDate"" и ""description"" (при этом все поля сохраняются, без изменений).
- ""after"": новое значение поля после улучшения. Тип должен соответствовать ""before"". Если тип ""experience"", то ""after"" должен быть массивом объектов, где все поля остаются прежними, кроме поля ""description"", которое изменяется на улучшенное. Если тип ""skills"", то ""after"" должен быть массивом строк с обновлённым списком навыков.
- ""reasoning"": обязательное текстовое обоснование предложения, объясняющее, почему внесённое изменение улучшит резюме. Это поле не должно быть пустым.

ВАЖНЫЕ ПРАВИЛА:
1. Не предлагай улучшения для поля ""education"" — пропускай его.
2. Для поля ""experience"": улучшать можно только описание (""description""). ""before"" должно быть исходным массивом объектов опыта, а ""after"" — таким же массивом, где, если предложение применяется, значение ""description"" изменено, а остальные поля (company, position, startDate, endDate) остаются без изменений.
3. Для поля ""skills"": если предложение относится к навыкам, то ""before"" — это массив исходных навыков (например, [""VueJS"", ""React"", ""Angular"", ""Rest API"", ""Composition API""]), а ""after"" — массив с этими же навыками, дополненный или изменённый (например, добавлен новый навык, если это уместно). Не возвращай пустые массивы.
4. Для полей ""title"", ""job"" и ""description"": ""before"" и ""after"" должны быть строками. Предложение должно быть конкретным и адаптированным к входным данным.
5. Поле ""reasoning"" обязательно должно содержать обоснование (например, ""Уточнение специализации поможет рекрутерам быстрее понять квалификацию кандидата.""). Оно не должно быть пустым или равным ""null"".
6. Результат ДОЛЖЕН быть строго валидным JSON без лишних символов, без Markdown, без комментариев.
7. Старайся довести изначальные поля резюме до идеала, достойного высококвалифицированного специалиста, которого захотят видеть в любой компании. Не бойся предлагать смелые изменения, если они обоснованы. Также логически сопоставляй свои изменения чтобы не было противоречий между ними. Всё должно быть логично и идеально чтобы в итоге получалось резюме, которое будет вызывать восторг у рекрутера.

Пример ожидаемого объекта для поля ""skills"":
{
  ""id"": ""sug-2"",
  ""type"": ""skills"",
  ""title"": ""Обновление навыков"",
  ""description"": ""Добавление нового навыка 'Webpack' для современного фронтенда."",
  ""confidence"": 0.8,
  ""before"": [""VueJS"", ""React"", ""Angular"", ""Rest API"", ""Composition API""],
  ""after"": [""VueJS"", ""React"", ""Angular"", ""Rest API"", ""Composition API"", ""Webpack""],
  ""reasoning"": ""Добавление 'Webpack' демонстрирует знание современных инструментов сборки, что повысит конкурентоспособность резюме.""
}

Пример ожидаемого объекта для поля ""experience"":
{
  ""id"": ""sug-3"",
  ""type"": ""experience"",
  ""title"": ""Улучшение описания опыта"",
  ""description"": ""Доработать описание достижения для большей конкретики."",
  ""confidence"": 0.85,
  ""before"": [
    {
      ""company"": ""Yandex"",
      ""position"": ""Frontend-разработчик"",
      ""startDate"": ""2015-03"",
      ""endDate"": ""2024-04"",
      ""description"": ""Оптимизировал фронт""
    }
  ],
  ""after"": [
    {
      ""company"": ""Yandex"",
      ""position"": ""Frontend-разработчик"",
      ""startDate"": ""2015-03"",
      ""endDate"": ""2024-04"",
      ""description"": ""Оптимизировал фронт и внедрил современные методы оптимизации, что повысило производительность на 20%.""
    }
  ],
  ""reasoning"": ""Добавление конкретных результатов в описание опыта делает достижения более измеримыми и привлекательными для рекрутеров.""
}
".Trim();

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

            var payloadJson = JsonConvert.SerializeObject(payload);

            int attempts = 0;
            List<SuggestionDto> suggestions = null;
            while (attempts < 3)
            {
                attempts++;
                Console.WriteLine($"Попытка {attempts}: Отправка запроса к ИИ...");

                // Создаем новый объект запроса для каждой попытки
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, gigachatUrl)
                {
                    Content = new StringContent(payloadJson, Encoding.UTF8, "application/json")
                };
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                try
                {
                    dynamic responseJson = JsonConvert.DeserializeObject(responseContent);
                    string messageContent = responseJson.choices[0]?.message?.content;

                    if (string.IsNullOrWhiteSpace(messageContent))
                    {
                        Console.WriteLine("Ошибка: Пустой ответ от ИИ.");
                        continue;
                    }

                    // Удаляем Markdown-обертку
                    messageContent = Regex.Replace(messageContent.Trim(), @"^```json\s*|\s*```$", "",
                        RegexOptions.Singleline);
                    suggestions = JsonConvert.DeserializeObject<List<SuggestionDto>>(messageContent);

                    if (suggestions != null)
                    {
                        Console.WriteLine("✅ Успешно получен корректный JSON.");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка парсинга JSON (попытка {attempts}): {ex.Message}");
                }

                if (attempts >= 3)
                    return Result<List<SuggestionDto>>.Failure("Не удалось получить корректный JSON после 3 попыток.");
            }

            if (suggestions == null)
                return Result<List<SuggestionDto>>.Failure("Ошибка: suggestions == null после 3 попыток.");

            // Обработка каждого предложения
            foreach (var suggestion in suggestions)
            {
                if (string.IsNullOrWhiteSpace(suggestion.Reasoning) || suggestion.Reasoning.Trim().ToLower() == "null")
                    suggestion.Reasoning = "Обоснование не предоставлено.";

                if (suggestion.Before is string beforeStr)
                    suggestion.Before = CleanString(beforeStr);

                if (suggestion.After is string afterStr)
                    suggestion.After = CleanString(afterStr);

                if (suggestion.Type?.Trim().ToLower() == "skills")
                {
                    List<string> ProcessSkillsField(object fieldValue)
                    {
                        if (fieldValue is string strValue && strValue.Trim().StartsWith("["))
                        {
                            try
                            {
                                return JsonConvert.DeserializeObject<List<string>>(CleanString(strValue));
                            }
                            catch
                            {
                                return new List<string>();
                            }
                        }

                        if (fieldValue is IEnumerable<object> enumerable)
                        {
                            var list = new List<object>(enumerable);
                            if (list.Count == 1 && list[0] is string singleStr && singleStr.Trim().StartsWith("["))
                            {
                                try
                                {
                                    return JsonConvert.DeserializeObject<List<string>>(CleanString(singleStr));
                                }
                                catch
                                {
                                    return new List<string>();
                                }
                            }

                            return list.ConvertAll(item => item?.ToString() ?? "");
                        }

                        return new List<string> { fieldValue?.ToString() };
                    }

                    suggestion.Before = ProcessSkillsField(suggestion.Before);
                    suggestion.After = ProcessSkillsField(suggestion.After);
                }

                if (suggestion.Type?.Trim().ToLower() == "experience")
                {
                    if (suggestion.Before is string beforeExpStr && beforeExpStr.Trim().StartsWith("["))
                    {
                        try
                        {
                            suggestion.Before =
                                JsonConvert.DeserializeObject<List<ExperienceDto>>(CleanString(beforeExpStr));
                        }
                        catch
                        {
                            suggestion.Before = new List<ExperienceDto>();
                        }
                    }
                    else if (suggestion.Before is Newtonsoft.Json.Linq.JArray jBefore)
                    {
                        suggestion.Before = jBefore.ToObject<List<ExperienceDto>>();
                    }
                    else if (!(suggestion.Before is List<ExperienceDto>))
                    {
                        try
                        {
                            suggestion.Before = new List<ExperienceDto>
                            {
                                JsonConvert.DeserializeObject<ExperienceDto>(
                                    JsonConvert.SerializeObject(suggestion.Before))
                            };
                        }
                        catch
                        {
                            suggestion.Before = new List<ExperienceDto>();
                        }
                    }

                    if (suggestion.After is string afterExpStr && afterExpStr.Trim().StartsWith("["))
                    {
                        try
                        {
                            suggestion.After =
                                JsonConvert.DeserializeObject<List<ExperienceDto>>(CleanString(afterExpStr));
                        }
                        catch
                        {
                            suggestion.After = new List<ExperienceDto>();
                        }
                    }
                    else if (suggestion.After is Newtonsoft.Json.Linq.JArray jAfter)
                    {
                        suggestion.After = jAfter.ToObject<List<ExperienceDto>>();
                    }
                    else if (!(suggestion.After is List<ExperienceDto>))
                    {
                        try
                        {
                            suggestion.After = new List<ExperienceDto>
                            {
                                JsonConvert.DeserializeObject<ExperienceDto>(
                                    JsonConvert.SerializeObject(suggestion.After))
                            };
                        }
                        catch
                        {
                            suggestion.After = new List<ExperienceDto>();
                        }
                    }
                }
            }

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
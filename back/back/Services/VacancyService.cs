using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Back.Services
{
    public class VacancyService
    {
        private readonly HttpClient _httpClient;

        public VacancyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string NormalizeUrl(string url)
        {
            // Если схема отсутствует, добавляем https://
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "https://" + url;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return null;
            }

            // Удаляем "www." из домена, если оно есть
            var host = uri.Host.Replace("www.", "");

            // Ожидаем, что путь имеет вид "/vacancy/<id>"
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length >= 2 && segments[0].Equals("vacancy", StringComparison.OrdinalIgnoreCase))
            {
                var vacancyId = segments[1];
                return $"https://hh.ru/vacancy/{vacancyId}";
            }
            else
            {
                return null;
            }
        }

        public async Task<string> ParsePageAsync(string url)
        {
            try
            {
                // Устанавливаем заголовок User-Agent аналогично Python-коду
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/90.0.4430.93 Safari/537.36");

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Парсим HTML с помощью HtmlAgilityPack и получаем текст
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(content);
                var text = htmlDoc.DocumentNode.InnerText.Trim();

                return text;
            }
            catch (HttpRequestException ex)
            {
                return $"Ошибка: {ex.Message}";
            }
        }
    }
}

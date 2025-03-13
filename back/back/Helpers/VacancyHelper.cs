using HtmlAgilityPack;

namespace back.Helpers;

public class VacancyHelper
{
    // Функция для нормализации URL
    public static string NormalizeUrl(string url)
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

        // Удаляем "www." из домена
        var host = uri.Host.Replace("www.", "");

        // Проверяем, что путь имеет вид "/vacancy/<id>"
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

    // Асинхронная функция для получения текста страницы по URL
    public static async Task<string> ParseVacancyText(string url)
    {
        string normalizedUrl = NormalizeUrl(url);
        if (normalizedUrl == null)
        {
            return "Ошибка: не удалось нормализовать URL";
        }

        try
        {
            using var httpClient = new HttpClient();

            // Устанавливаем заголовок User-Agent аналогично Python-коду
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/90.0.4430.93 Safari/537.36");

            var response = await httpClient.GetAsync(normalizedUrl);
            response.EnsureSuccessStatusCode();
            string htmlContent = await response.Content.ReadAsStringAsync();

            // Парсим HTML и извлекаем текст с помощью HtmlAgilityPack
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);
            var text = doc.DocumentNode.InnerText.Trim();
            return text;
        }
        catch (Exception ex)
        {
            return $"Ошибка: {ex.Message}";
        }
    }
}
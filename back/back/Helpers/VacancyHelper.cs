using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace back.Helpers
{
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

        // Асинхронная функция для получения чистой информации о вакансии
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
                // Устанавливаем заголовок User-Agent
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/90.0.4430.93 Safari/537.36");

                var response = await httpClient.GetAsync(normalizedUrl);
                response.EnsureSuccessStatusCode();
                string htmlContent = await response.Content.ReadAsStringAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // Извлекаем необходимые элементы по data-qa атрибутам и структуре страницы
                var title = doc.DocumentNode.SelectSingleNode("//h1[@data-qa='vacancy-title']")?.InnerText.Trim();
                var salary = doc.DocumentNode.SelectSingleNode("//div[@data-qa='vacancy-salary']")?.InnerText.Trim();
                var experience = doc.DocumentNode.SelectSingleNode("//span[@data-qa='vacancy-experience']")?.InnerText.Trim();
                var employment = doc.DocumentNode.SelectSingleNode("//div[@data-qa='common-employment-text']//span")?.InnerText.Trim();
                var schedule = doc.DocumentNode.SelectSingleNode("//p[@data-qa='work-schedule-by-days-text']")?.InnerText.Trim();
                var workHours = doc.DocumentNode.SelectSingleNode("//div[@data-qa='working-hours-text']//span")?.InnerText.Trim();
                var workFormat = doc.DocumentNode.SelectSingleNode("//p[@data-qa='work-formats-text']")?.InnerText.Trim();
                var description = doc.DocumentNode.SelectSingleNode("//div[@data-qa='vacancy-description']")?.InnerText.Trim();

                // Собираем итоговую информацию
                var sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(title))
                {
                    sb.AppendLine(title);
                }
                if (!string.IsNullOrWhiteSpace(salary))
                {
                    sb.AppendLine(salary);
                }
                if (!string.IsNullOrWhiteSpace(experience))
                {
                    sb.AppendLine("Опыт работы: " + experience);
                }
                if (!string.IsNullOrWhiteSpace(employment))
                {
                    sb.AppendLine("Занятость: " + employment);
                }
                if (!string.IsNullOrWhiteSpace(schedule))
                {
                    sb.AppendLine(schedule);
                }
                if (!string.IsNullOrWhiteSpace(workHours))
                {
                    sb.AppendLine(workHours);
                }
                if (!string.IsNullOrWhiteSpace(workFormat))
                {
                    sb.AppendLine(workFormat);
                }
                if (!string.IsNullOrWhiteSpace(description))
                {
                    sb.AppendLine("\nОписание вакансии:");
                    sb.AppendLine(description);
                }

                string result = sb.ToString();
                // Убираем лишние пробелы и переносы строк
                result = Regex.Replace(result, @"\s+", " ").Trim();
                return result;
            }
            catch (Exception ex)
            {
                return $"Ошибка: {ex.Message}";
            }
        }
    }
}

using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using back.DTOs;

namespace back.Helpers
{
    public class VacancyHelper
    {
        public static string NormalizeUrl(string url)
        {
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "https://" + url;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return null;
            }

            var host = uri.Host.Replace("www.", "");

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

        public static async Task<VacancyDto> ParseVacancyAsync(string url, HttpClient httpClient)
        {
            string normalizedUrl = NormalizeUrl(url);
            if (normalizedUrl == null)
            {
                throw new ArgumentException("Ошибка: не удалось нормализовать URL");
            }

            // Очистка заголовков User-Agent, чтобы избежать дублирования
            httpClient.DefaultRequestHeaders.UserAgent.Clear();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/90.0.4430.93 Safari/537.36");
    
            var response = await httpClient.GetAsync(normalizedUrl);
            response.EnsureSuccessStatusCode();
            string htmlContent = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var vacancyDto = new VacancyDto
            {
                Title = doc.DocumentNode.SelectSingleNode("//h1[@data-qa='vacancy-title']")?.InnerText.Trim(),
                Salary = doc.DocumentNode.SelectSingleNode("//div[@data-qa='vacancy-salary']")?.InnerText.Trim(),
                Experience = doc.DocumentNode.SelectSingleNode("//span[@data-qa='vacancy-experience']")?.InnerText.Trim(),
                Employment = doc.DocumentNode.SelectSingleNode("//div[@data-qa='common-employment-text']//span")?.InnerText.Trim(),
                Schedule = doc.DocumentNode.SelectSingleNode("//p[@data-qa='work-schedule-by-days-text']")?.InnerText.Trim(),
                WorkHours = doc.DocumentNode.SelectSingleNode("//div[@data-qa='working-hours-text']//span")?.InnerText.Trim(),
                WorkFormat = doc.DocumentNode.SelectSingleNode("//p[@data-qa='work-formats-text']")?.InnerText.Trim(),
                Description = doc.DocumentNode.SelectSingleNode("//div[@data-qa='vacancy-description']")?.InnerText.Trim()
            };

            CleanVacancyDto(vacancyDto);

            return vacancyDto;
        }


        private static void CleanVacancyDto(VacancyDto vacancy)
        {
            vacancy.Title = Regex.Replace(vacancy.Title ?? string.Empty, @"\s+", " ").Trim();
            vacancy.Salary = Regex.Replace(vacancy.Salary ?? string.Empty, @"\s+", " ").Trim();
            vacancy.Experience = Regex.Replace(vacancy.Experience ?? string.Empty, @"\s+", " ").Trim();
            vacancy.Employment = Regex.Replace(vacancy.Employment ?? string.Empty, @"\s+", " ").Trim();
            vacancy.Schedule = Regex.Replace(vacancy.Schedule ?? string.Empty, @"\s+", " ").Trim();
            vacancy.WorkHours = Regex.Replace(vacancy.WorkHours ?? string.Empty, @"\s+", " ").Trim();
            vacancy.WorkFormat = Regex.Replace(vacancy.WorkFormat ?? string.Empty, @"\s+", " ").Trim();
            vacancy.Description = Regex.Replace(vacancy.Description ?? string.Empty, @"\s+", " ").Trim();
        }
    }
}

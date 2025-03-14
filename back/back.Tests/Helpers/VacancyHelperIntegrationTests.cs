using back.Helpers;

namespace back.Tests.Helpers
{
    public class VacancyHelperIntegrationTests
    {
        // Этот тест отправляет реальный HTTP-запрос к hh.ru.
        [Fact(Skip = "Интеграционный тест: требует доступа к сети и актуального вакансия URL.")]
        public async Task ParseVacancyText_RealRequest_ReturnsValidContent()
        {
            string vacancyUrl = "https://hh.ru/vacancy/50000950"; 
            
            // Выполнение запроса и парсинг вакансии.
            string result = await VacancyHelper.ParseVacancyText(vacancyUrl);
            
            Assert.False(string.IsNullOrEmpty(result), "Результат не должен быть пустым.");
            
            Assert.Contains("Вакансия", result, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
using back.Helpers;
using back.DTOs;
using Xunit.Abstractions;

namespace back.Tests.Helpers
{
    public class VacancyHelperIntegrationTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public VacancyHelperIntegrationTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ParseVacancyAsync_RealData_ReturnsVacancyData()
        {
            string url = "https://hh.ru/vacancy/118719211?from=applicant_recommended&hhtmFrom=main";

            using HttpClient httpClient = new HttpClient();
            
            VacancyDto vacancy = await VacancyHelper.ParseVacancyAsync(url, httpClient);

            // Выполняем простые проверки, чтобы убедиться, что данные получены
            Assert.NotNull(vacancy);
            Assert.False(string.IsNullOrWhiteSpace(vacancy.Title));
            Assert.False(string.IsNullOrWhiteSpace(vacancy.Description));

            _testOutputHelper.WriteLine("Title: " + vacancy.Title);
            _testOutputHelper.WriteLine("Salary: " + vacancy.Salary);
            _testOutputHelper.WriteLine("Experience: " + vacancy.Experience);
            _testOutputHelper.WriteLine("Employment: " + vacancy.Employment);
            _testOutputHelper.WriteLine("Schedule: " + vacancy.Schedule);
            _testOutputHelper.WriteLine("WorkHours: " + vacancy.WorkHours);
            _testOutputHelper.WriteLine("WorkFormat: " + vacancy.WorkFormat);
            _testOutputHelper.WriteLine("Description: " + vacancy.Description);
        }
    }
}
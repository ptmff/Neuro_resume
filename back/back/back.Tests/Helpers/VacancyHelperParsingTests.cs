using System;
using System.Net.Http;
using System.Threading.Tasks;
using back.Application.DTOs;
using back.Infrastructure.Services;
using RichardSzalay.MockHttp;
using Xunit;

namespace back.Tests.Helpers
{
    public class VacancyHelperParsingTests
    {
        // Вспомогательный метод для создания мокированного HttpClient
        private HttpClient CreateMockHttpClient(string normalizedUrl, string fakeHtml)
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(normalizedUrl)
                .Respond("text/html", fakeHtml);
            var httpClient = mockHttp.ToHttpClient();
            // Не устанавливаем User-Agent здесь, так как он задается в методе ParseVacancyAsync
            return httpClient;
        }


        // Вспомогательный метод для построения HTML с различными полями
        private string BuildHtml(
            string title = null,
            string salary = null,
            string experience = null,
            string employment = null,
            string schedule = null,
            string workingHours = null,
            string workFormat = null,
            string description = null)
        {
            string html = "<html><body>";
            if (title != null)
                html += $"<h1 data-qa='vacancy-title'>{title}</h1>";
            if (salary != null)
                html += $"<div data-qa='vacancy-salary'>{salary}</div>";
            if (experience != null)
                html += $"<span data-qa='vacancy-experience'>{experience}</span>";
            if (employment != null)
                html += $"<div data-qa='common-employment-text'><span>{employment}</span></div>";
            if (schedule != null)
                html += $"<p data-qa='work-schedule-by-days-text'>{schedule}</p>";
            if (workingHours != null)
                html += $"<div data-qa='working-hours-text'><span>{workingHours}</span></div>";
            if (workFormat != null)
                html += $"<p data-qa='work-formats-text'>{workFormat}</p>";
            if (description != null)
                html += $"<div data-qa='vacancy-description'><p>{description}</p></div>";
            html += "</body></html>";
            return html;
        }

        // Тест: все поля присутствуют
        [Fact]
        public async Task ParseVacancyAsync_AllFieldsPresent_ReturnsParsedVacancyData()
        {
            // Arrange
            string vacancyId = "123456";
            string url = $"hh.ru/vacancy/{vacancyId}";
            string normalizedUrl = $"https://hh.ru/vacancy/{vacancyId}";

            string title = "Заголовок вакансии";
            string salary = "50 000 - 70 000 руб.";
            string experience = "3 года";
            string employment = "Полная занятость";
            string schedule = "Понедельник - пятница";
            string workingHours = "9:00 - 18:00";
            string workFormat = "Офис";
            string description = "Описание вакансии";

            string fakeHtml = BuildHtml(title, salary, experience, employment, schedule, workingHours, workFormat, description);
            var httpClient = CreateMockHttpClient(normalizedUrl, fakeHtml);

            // Act
            VacancyDto result = await VacancyHelper.ParseVacancyAsync(url, httpClient);

            // Assert
            Assert.Equal(title, result.Title);
            Assert.Equal(salary, result.Salary);
            Assert.Equal(experience, result.Experience);
            Assert.Equal(employment, result.Employment);
            Assert.Equal(schedule, result.Schedule);
            Assert.Equal(workingHours, result.WorkHours);
            Assert.Equal(workFormat, result.WorkFormat);
            Assert.Equal(description, result.Description);
        }

        // Тест: отсутствует заголовок вакансии
[Fact]
public async Task ParseVacancyAsync_TitleMissing_ReturnsDataWithoutTitle()
{
    // Arrange
    string vacancyId = "123456";
    string url = $"hh.ru/vacancy/{vacancyId}";
    string normalizedUrl = $"https://hh.ru/vacancy/{vacancyId}";

    // Заголовок отсутствует (null)
    string title = null;
    string salary = "60 000 руб.";
    string experience = "5 лет";
    string employment = "Частичная занятость";
    string schedule = "Гибкий график";
    string workingHours = "10:00 - 16:00";
    string workFormat = "Удаленная работа";
    string description = "Описание вакансии без заголовка";

    string fakeHtml = BuildHtml(title, salary, experience, employment, schedule, workingHours, workFormat, description);
    var httpClient = CreateMockHttpClient(normalizedUrl, fakeHtml);

    // Act
    VacancyDto result = await VacancyHelper.ParseVacancyAsync(url, httpClient);

    // Assert
    Assert.True(string.IsNullOrEmpty(result.Title));
    Assert.Equal(salary, result.Salary);
    Assert.Equal(experience, result.Experience);
    Assert.Equal(employment, result.Employment);
    Assert.Equal(schedule, result.Schedule);
    Assert.Equal(workingHours, result.WorkHours);
    Assert.Equal(workFormat, result.WorkFormat);
    Assert.Equal(description, result.Description);
}

// Тест: отсутствует информация о зарплате
[Fact]
public async Task ParseVacancyAsync_SalaryMissing_ReturnsDataWithoutSalary()
{
    // Arrange
    string vacancyId = "654321";
    string url = $"hh.ru/vacancy/{vacancyId}";
    string normalizedUrl = $"https://hh.ru/vacancy/{vacancyId}";

    string title = "Вакансия без зарплаты";
    string salary = null;
    string experience = "2 года";
    string employment = "Полная занятость";
    string schedule = "Понедельник - пятница";
    string workingHours = "9:00 - 18:00";
    string workFormat = "Офис";
    string description = "Описание вакансии без зарплаты";

    string fakeHtml = BuildHtml(title, salary, experience, employment, schedule, workingHours, workFormat, description);
    var httpClient = CreateMockHttpClient(normalizedUrl, fakeHtml);

    // Act
    VacancyDto result = await VacancyHelper.ParseVacancyAsync(url, httpClient);

    // Assert
    Assert.Equal(title, result.Title);
    Assert.True(string.IsNullOrEmpty(result.Salary));
    Assert.Equal(experience, result.Experience);
    Assert.Equal(employment, result.Employment);
    Assert.Equal(schedule, result.Schedule);
    Assert.Equal(workingHours, result.WorkHours);
    Assert.Equal(workFormat, result.WorkFormat);
    Assert.Equal(description, result.Description);
}

// Тест: отсутствует информация об опыте
[Fact]
public async Task ParseVacancyAsync_ExperienceMissing_ReturnsDataWithoutExperience()
{
    // Arrange
    string vacancyId = "789012";
    string url = $"hh.ru/vacancy/{vacancyId}";
    string normalizedUrl = $"https://hh.ru/vacancy/{vacancyId}";

    string title = "Вакансия без опыта";
    string salary = "40 000 руб.";
    string experience = null;
    string employment = "Полная занятость";
    string schedule = "График работы: 5/2";
    string workingHours = "8:00 - 17:00";
    string workFormat = "Офис";
    string description = "Описание вакансии без опыта";

    string fakeHtml = BuildHtml(title, salary, experience, employment, schedule, workingHours, workFormat, description);
    var httpClient = CreateMockHttpClient(normalizedUrl, fakeHtml);

    // Act
    VacancyDto result = await VacancyHelper.ParseVacancyAsync(url, httpClient);

    // Assert
    Assert.Equal(title, result.Title);
    Assert.Equal(salary, result.Salary);
    Assert.True(string.IsNullOrEmpty(result.Experience));
    Assert.Equal(employment, result.Employment);
    Assert.Equal(schedule, result.Schedule);
    Assert.Equal(workingHours, result.WorkHours);
    Assert.Equal(workFormat, result.WorkFormat);
    Assert.Equal(description, result.Description);
}

// Тест: отсутствует описание вакансии
[Fact]
public async Task ParseVacancyAsync_DescriptionMissing_ReturnsDataWithoutDescription()
{
    // Arrange
    string vacancyId = "345678";
    string url = $"hh.ru/vacancy/{vacancyId}";
    string normalizedUrl = $"https://hh.ru/vacancy/{vacancyId}";

    string title = "Вакансия без описания";
    string salary = "55 000 руб.";
    string experience = "1 год";
    string employment = "Полная занятость";
    string schedule = "Стандартный график";
    string workingHours = "9:00 - 18:00";
    string workFormat = "Офис";
    string description = null;

    string fakeHtml = BuildHtml(title, salary, experience, employment, schedule, workingHours, workFormat, description);
    var httpClient = CreateMockHttpClient(normalizedUrl, fakeHtml);

    // Act
    VacancyDto result = await VacancyHelper.ParseVacancyAsync(url, httpClient);

    // Assert
    Assert.Equal(title, result.Title);
    Assert.Equal(salary, result.Salary);
    Assert.Equal(experience, result.Experience);
    Assert.Equal(employment, result.Employment);
    Assert.Equal(schedule, result.Schedule);
    Assert.Equal(workingHours, result.WorkHours);
    Assert.Equal(workFormat, result.WorkFormat);
    Assert.True(string.IsNullOrEmpty(result.Description));
}

        // Тест: все поля отсутствуют
        [Fact]
        public async Task ParseVacancyAsync_AllFieldsMissing_ReturnsEmptyResult()
        {
            // Arrange
            string vacancyId = "000000";
            string url = $"hh.ru/vacancy/{vacancyId}";
            string normalizedUrl = $"https://hh.ru/vacancy/{vacancyId}";

            // Генерируем HTML без каких-либо данных
            string fakeHtml = BuildHtml();
            var httpClient = CreateMockHttpClient(normalizedUrl, fakeHtml);

            // Act
            VacancyDto result = await VacancyHelper.ParseVacancyAsync(url, httpClient);

            // Assert: если данные отсутствуют, итоговый результат должен быть пустым или почти пустым
            Assert.True(string.IsNullOrWhiteSpace(result.Title) &&
                        string.IsNullOrWhiteSpace(result.Salary) &&
                        string.IsNullOrWhiteSpace(result.Experience) &&
                        string.IsNullOrWhiteSpace(result.Employment) &&
                        string.IsNullOrWhiteSpace(result.Schedule) &&
                        string.IsNullOrWhiteSpace(result.WorkHours) &&
                        string.IsNullOrWhiteSpace(result.WorkFormat) &&
                        string.IsNullOrWhiteSpace(result.Description));
        }
    }
}

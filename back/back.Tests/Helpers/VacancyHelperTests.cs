using back.Helpers;

namespace back.Tests.Helpers
{
    public class VacancyHelperTests
    {
        [Theory]
        [InlineData("https://hh.ru/vacancy/117843700?from=main&utm_source=headhunter&utm_medium=rainbow_catalog&utm_campaign=vacancy_of_the_day_to", "https://hh.ru/vacancy/123456")]
        [InlineData("https://hh.ru/vacancy/87661340?from=main&utm_source=headhunter&utm_medium=rainbow_catalog&utm_campaign=vacancy_of_the_day_to", "https://hh.ru/vacancy/123456")]
        public void NormalizeUrl_ValidVacancyUrl_ReturnsNormalizedUrl(string input, string expected)
        {
            // Act
            var result = VacancyHelper.NormalizeUrl(input);
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("https://hh.ru/someotherpath/123456")]
        [InlineData("not-a-url")]
        public void NormalizeUrl_InvalidVacancyUrl_ReturnsNull(string input)
        {
            // Act
            var result = VacancyHelper.NormalizeUrl(input);
            
            // Assert
            Assert.Null(result);
        }
    }
}
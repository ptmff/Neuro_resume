

using back.Infrastructure.Services;

namespace back.Tests.Helpers
{
    public class VacancyHelperTests
    {
        [Theory]
        [InlineData("https://hh.ru/vacancy/117843700?from=main&utm_source=headhunter&utm_medium=rainbow_catalog&utm_campaign=vacancy_of_the_day_to", "https://hh.ru/vacancy/117843700")]
        [InlineData("https://hh.ru/vacancy/87661340?from=main&utm_source=headhunter&utm_medium=rainbow_catalog&utm_campaign=vacancy_of_the_day_to", "https://hh.ru/vacancy/87661340")]
        [InlineData("https://hh.ru/vacancy/118344273?query=c%23&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/118344273")]
        [InlineData("https://hh.ru/vacancy/118113283?query=c%23&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/118113283")]
        [InlineData("https://hh.ru/vacancy/117479724?query=c%23&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/117479724")]
        [InlineData("https://hh.ru/vacancy/118330473?query=c%23&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/118330473")]
        [InlineData("https://hh.ru/vacancy/118186764?query=c%23&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/118186764")]
        [InlineData("https://hh.ru/vacancy/117841460?query=c%23&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/117841460")]
        [InlineData("https://hh.ru/vacancy/117748532?hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/117748532")]
        [InlineData("https://hh.ru/vacancy/118153283?from=applicant_recommended&hhtmFrom=main", "https://hh.ru/vacancy/118153283")]
        [InlineData("https://hh.ru/vacancy/118294967?from=applicant_recommended&hhtmFrom=main", "https://hh.ru/vacancy/118294967")]
        [InlineData("https://hh.ru/vacancy/117964150?from=applicant_recommended&hhtmFrom=main", "https://hh.ru/vacancy/117964150")]
        [InlineData("https://hh.ru/vacancy/117785817?from=applicant_recommended&hhtmFrom=main", "https://hh.ru/vacancy/117785817")]
        [InlineData("https://hh.ru/vacancy/118301701?from=applicant_recommended&hhtmFrom=main", "https://hh.ru/vacancy/118301701")]
        [InlineData("https://hh.ru/vacancy/117727663?query=js+dev&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/117727663")]
        [InlineData("https://hh.ru/vacancy/117846056?query=js+dev&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/117846056")]
        [InlineData("https://hh.ru/vacancy/118241450?query=js+dev&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/118241450")]
        [InlineData("https://hh.ru/vacancy/117753560?query=js+dev&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/117753560")]
        [InlineData("https://hh.ru/vacancy/118209729?query=js+dev&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/118209729")]
        [InlineData("https://hh.ru/vacancy/118009540?query=js+dev&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/118009540")]
        [InlineData("https://hh.ru/vacancy/117640838?query=js+dev&hhtmFrom=vacancy_search_list", "https://hh.ru/vacancy/117640838")]
        public void NormalizeUrl_ValidVacancyUrl_ReturnsNormalizedUrl(string input, string expected)
        {
            // Act
            var result = VacancyHelper.NormalizeUrl(input);
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
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
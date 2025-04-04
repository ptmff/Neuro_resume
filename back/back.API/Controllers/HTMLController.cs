using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using back.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;

namespace back.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        [HttpPost("generate-html")]
        public async Task<IActionResult> GenerateHtml([FromBody] ResumeDto resumeDto)
        {
            if (resumeDto == null)
            {
                return BadRequest("Неверные данные резюме.");
            }

            string htmlTemplate = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>{resumeDto.Title}</title>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        h1, h2 {{ color: #2c3e50; }}
        ul {{ list-style-type: none; padding: 0; }}
        li {{ margin-bottom: 5px; }}
    </style>
</head>
<body>
    <h1>{resumeDto.Title}</h1>
    <p><strong>Дата:</strong> {resumeDto.Date.ToShortDateString()}</p>
    <p><strong>Должность:</strong> {resumeDto.Job}</p>
    <p><strong>Город:</strong> {resumeDto.City}</p>
    <p><strong>Описание:</strong> {resumeDto.Description}</p>
    <h2>Навыки</h2>
    <ul>
        {string.Join(Environment.NewLine, resumeDto.Skills.Select(s => $"<li>{s}</li>"))}
    </ul>
    <h2>Опыт</h2>
    <ul>
        {string.Join(Environment.NewLine, resumeDto.Experience.Select(exp =>
            $"<li>{exp.Position} в {exp.Company} ({exp.StartDate} - {exp.EndDate})</li>"))}
    </ul>
</body>
</html>";

            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            var launchOptions = new LaunchOptions { Headless = true };

            using (var browser = await Puppeteer.LaunchAsync(launchOptions))
            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(htmlTemplate);

                string renderedHtml = await page.GetContentAsync();

                byte[] fileBytes = Encoding.UTF8.GetBytes(renderedHtml);
                return File(fileBytes, "text/html", "resume.html");
            }
        }
    }
}

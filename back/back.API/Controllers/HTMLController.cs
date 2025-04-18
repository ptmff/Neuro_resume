using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using back.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using back.Application.Interfaces;
using back.Application.Services; 

namespace back.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IHtmlGenerator _htmlGenerator;
        private readonly IPdfGenerator _pdfGenerator;

        public ResumeController()
        {
            _htmlGenerator = new HtmlGenerator();
            _pdfGenerator = new PdfGenerator();
        }

        [HttpPost("generate-pdf")]
        public async Task<IActionResult> GeneratePdf([FromBody] ResumeDto resumeDto)
        {
            if (resumeDto == null)
                return BadRequest("Неверные данные резюме.");

            string htmlContent = _htmlGenerator.Generate(resumeDto);

            byte[] pdfFileBytes = await _pdfGenerator.GeneratePdfAsync(htmlContent);
            return File(pdfFileBytes, "application/pdf", "resume.pdf");
        }
    }

}

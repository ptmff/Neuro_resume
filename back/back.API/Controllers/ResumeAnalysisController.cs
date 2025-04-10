using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using back.Application.DTOs;
using back.Application.Interfaces;
using back.Application.Common;
using back.API.Services; // для IUserContext

namespace back.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResumeAnalysisController : ControllerBase
{
    private readonly IResumeAnalysisService _analysisService;
    private readonly IUserContext _userContext;

    public ResumeAnalysisController(IResumeAnalysisService analysisService, IUserContext userContext)
    {
        _analysisService = analysisService;
        _userContext = userContext;
    }

    // POST: /api/resumeanalysis/analyze
    [HttpPost("analyze")]
    public async Task<IActionResult> AnalyzeResume([FromBody] ResumeDtoForAi resume)
    {
        var result = await _analysisService. AnalyzeResumeAsync(resume);
        return result.ToActionResult();
    }
    
    [HttpPost("analyzeVacancy")]
    public async Task<IActionResult> AnalyzeResumeVacancy([FromBody] ResumeVacancyAnalysisRequestDto request)
    {
        var result = await _analysisService.AnalyzeResumeVacancyAsync(request);
        return result.ToActionResult();
    }
}
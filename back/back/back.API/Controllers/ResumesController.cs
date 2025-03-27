using back.API.Services;
using back.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back.Application.DTOs;            // DTO для резюме
using back.Application.Interfaces;        // Интерфейс IResumeService


namespace back.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResumesController : ControllerBase
{
    private readonly IResumeService _resumeService;
    private readonly IUserContext _userContext;
        
    public ResumesController(IResumeService resumeService, IUserContext userContext)
    {
        _resumeService = resumeService;
        _userContext = userContext;
    }
        
    [HttpGet]
    public async Task<IActionResult> GetResumes()
    {
        var result = await _resumeService.GetUserResumes(_userContext.UserId);
        return result.ToActionResult();
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetResume(int id)
    {
        var result = await _resumeService.GetResumeById(_userContext.UserId, id);
        return result.ToActionResult();
    }
        
    [HttpPost]
    public async Task<IActionResult> PostResume([FromBody] ResumeDto resumeDto)
    {
        var result = await _resumeService.CreateResume(_userContext.UserId, resumeDto);
        return result.ToActionResultCreated(nameof(GetResumes), r => r.Id);
    }
        
    [HttpPut("{id}")]
    public async Task<IActionResult> PutResume(int id, [FromBody] ResumeDto resumeDto)
    {
        var result = await _resumeService.UpdateResume(_userContext.UserId, id, resumeDto);
        return result.ToActionResult();
    }
        
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResume(int id)
    {
        var result = await _resumeService.DeleteResume(_userContext.UserId, id);
        return result.ToActionResult();
    }
}
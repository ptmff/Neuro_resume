using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back.Data;
using back.Models;

namespace back.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResumesController : ControllerBase
{
    private readonly IResumeService _resumeService;
    private readonly IUserContext _userContext;

    public ResumesController(
        IResumeService resumeService,
        IUserContext userContext)
    {
        _resumeService = resumeService;
        _userContext = userContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Resume>>> GetResumes()
    {
        var result = await _resumeService.GetUserResumes(_userContext.UserId);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<ActionResult<Resume>> PostResume(ResumeDto resumeDto)
    {
        var result = await _resumeService.CreateResume(_userContext.UserId, resumeDto);
        return result.ToActionResultCreated(nameof(GetResumes), r => r.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutResume(int id, ResumeDto resumeDto)
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

// Новые интерфейсы и сервисы
public interface IResumeService
{
    Task<Result<IEnumerable<Resume>>> GetUserResumes(int userId);
    Task<Result<Resume>> CreateResume(int userId, ResumeDto dto);
    Task<Result> UpdateResume(int userId, int resumeId, ResumeDto dto);
    Task<Result> DeleteResume(int userId, int resumeId);
}

public interface IUserContext
{
    int UserId { get; }
}

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId => int.Parse(
        _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("User not authenticated"));
}

public class ResumeService : IResumeService
{
    private readonly AppDbContext _context;
    private readonly IAuthorizationService _authService;

    public ResumeService(
        AppDbContext context,
        IAuthorizationService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<Result<IEnumerable<Resume>>> GetUserResumes(int userId)
    {
        var resumes = await _context.Resumes
            .Where(r => r.UserId == userId)
            .ToListAsync();
        
        return Result<IEnumerable<Resume>>.Success(resumes);
    }

    public async Task<Result<Resume>> CreateResume(int userId, ResumeDto dto)
    {
        var resume = new Resume
        {
            Title = dto.Title,
            Date = dto.Date,
            Job = dto.Job,
            Skills = dto.Skills,
            UserId = userId
        };

        _context.Resumes.Add(resume);
        await _context.SaveChangesAsync();

        return Result<Resume>.Success(resume);
    }

    public async Task<Result> UpdateResume(int userId, int resumeId, ResumeDto dto)
    {
        var resume = await _context.Resumes.FindAsync(resumeId);
        if (resume == null) return Result.Failure("Resume not found");
        if (resume.UserId != userId) return Result.Failure("Forbidden");

        resume.Title = dto.Title;
        resume.Date = dto.Date;
        resume.Job = dto.Job;
        resume.Skills = dto.Skills;

        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteResume(int userId, int resumeId)
    {
        var resume = await _context.Resumes.FindAsync(resumeId);
        if (resume == null) return Result.Failure("Resume not found");
        if (resume.UserId != userId) return Result.Failure("Forbidden");

        _context.Resumes.Remove(resume);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}

// Расширения для преобразования Result в ActionResult
public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        return result.IsSuccess 
            ? new NoContentResult() 
            : GetErrorResult(result.Error);
    }

    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
        return result.IsSuccess 
            ? new OkObjectResult(result.Value) 
            : GetErrorResult(result.Error);
    }

    public static ActionResult<T> ToActionResultCreated<T>(
        this Result<T> result, 
        string actionName, 
        Func<T, object> routeValues)
    {
        return result.IsSuccess 
            ? new CreatedAtActionResult(
                actionName, 
                null, 
                routeValues(result.Value), 
                result.Value) 
            : GetErrorResult(result.Error);
    }

    private static ActionResult GetErrorResult(string error)
    {
        return error switch
        {
            "Forbidden" => new ForbidResult(),
            "Not found" => new NotFoundResult(),
            _ => new BadRequestObjectResult(error)
        };
    }
}
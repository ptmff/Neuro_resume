using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back.Data;
using back.Models;
using back.DTOs;
using back.Mappers;
using System.Security.Claims;

namespace back.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized("Пользователь не авторизован");

        if (!int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("Некорректный токен");

        var user = await _context.Users
            .Include(u => u.Resumes)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound("Пользователь не найден");

        var profile = new UserProfileDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            City = user.City,
            Photo = user.Photo,
            Education = (user.Education ?? new List<Education>())
                .Select(e => e.ToDto())
                .ToList(),
            MainResumeId = user.MainResumeId,
            Resumes = user.Resumes.Select(r => r.Id).ToList()
        };

        return Ok(profile);
    }

    [HttpPatch("profile")]
    public async Task<IActionResult> PatchProfile([FromBody] ProfileUpdateDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? throw new InvalidOperationException("User not authenticated"));

        var user = await _context.Users
            .Include(u => u.Education)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return NotFound("User not found");

        // Обновляем только переданные поля
        if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
        if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
        if (!string.IsNullOrEmpty(dto.Phone)) user.Phone = dto.Phone;
        if (!string.IsNullOrEmpty(dto.City)) user.City = dto.City;
        if (!string.IsNullOrEmpty(dto.Photo)) user.Photo = dto.Photo;
        if (dto.MainResumeId.HasValue) user.MainResumeId = dto.MainResumeId.Value;

        if (dto.Education != null)
        {
            user.Education = dto.Education.Select(e => new Education
            {
                Institution = e.Institution,
                Degree = e.Degree,
                Field = e.Field,
                StartYear = e.StartYear,
                EndYear = e.EndYear
            }).ToList();
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
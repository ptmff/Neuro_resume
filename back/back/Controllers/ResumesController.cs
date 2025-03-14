using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using back.Data;
using back.Models;

namespace back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResumesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ResumesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/resumes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Resume>>> GetResumes()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        return await _context.Resumes
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    // POST: api/resumes
    [HttpPost]
    public async Task<ActionResult<Resume>> PostResume(ResumeDto resumeDto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var resume = new Resume
        {
            Title = resumeDto.Title,
            Date = resumeDto.Date,
            Job = resumeDto.Job,
            Skills = resumeDto.Skills,
            UserId = userId
        };

        _context.Resumes.Add(resume);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetResumes), new { id = resume.Id }, resume);
    }

    // PUT: api/resumes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutResume(int id, Resume resume)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if (id != resume.Id)
        {
            return BadRequest();
        }

        var existingResume = await _context.Resumes.FindAsync(id);
        if (existingResume == null)
        {
            return NotFound();
        }

        if (existingResume.UserId != userId)
        {
            return Unauthorized();
        }

        existingResume.Title = resume.Title;
        existingResume.Date = resume.Date;
        existingResume.Job = resume.Job;
        existingResume.Skills = resume.Skills;

        _context.Entry(existingResume).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ResumeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/resumes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResume(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var resume = await _context.Resumes.FindAsync(id);
        
        if (resume == null)
        {
            return NotFound();
        }

        if (resume.UserId != userId)
        {
            return Unauthorized();
        }

        _context.Resumes.Remove(resume);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ResumeExists(int id)
    {
        return _context.Resumes.Any(e => e.Id == id);
    }
}
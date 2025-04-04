using back.Application.Common;
using back.Application.DTOs;
using back.Application.Interfaces;
using back.Domain.Entities;
using back.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace back.Application.Services;

public class ResumeService : IResumeService
    {
        private readonly AppDbContext _context;

        public ResumeService(AppDbContext context)
        {
            _context = context;
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
        var hasOtherResumes = await _context.Resumes.AnyAsync(r => r.UserId == userId);

        var resume = new Resume
        {
            Title = dto.Title,
            Date = dto.Date,
            Job = dto.Job,
            Skills = dto.Skills,
            City = dto.City,
            Template = dto.Template,
            Description = dto.Description,
            Experience = dto.Experience?.Select(e => new Experience
            {
                Company = e.Company,
                Position = e.Position,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Description = e.Description
            }).ToList(),
            UserId = userId
        };

        _context.Resumes.Add(resume);
        await _context.SaveChangesAsync();

        if (!hasOtherResumes)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.MainResumeId = resume.Id;
                await _context.SaveChangesAsync();
            }
        }

        return Result<Resume>.Success(resume);
    }

    public async Task<Result> UpdateResume(int userId, int resumeId, ResumeDto dto)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null)
                return Result.Failure("Resume not found");
            if (resume.UserId != userId)
                return Result.Failure("Forbidden");

            resume.Title = dto.Title;
            resume.Date = dto.Date;
            resume.Job = dto.Job;
            resume.Skills = dto.Skills;
            resume.City = dto.City;
            resume.Template = dto.Template;
            resume.Description = dto.Description;
            resume.Experience = dto.Experience.Select(e => new Experience
            {
                Company = e.Company,
                Position = e.Position,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Description = e.Description
            }).ToList();

            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteResume(int userId, int resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null)
                return Result.Failure("Resume not found");
            if (resume.UserId != userId)
                return Result.Failure("Forbidden");

            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        
        public async Task<Result<Resume>> GetResumeById(int userId, int resumeId)
        {
            var resume = await _context.Resumes
                .FirstOrDefaultAsync(r => r.Id == resumeId && r.UserId == userId);
            if (resume == null)
                return Result<Resume>.Failure("Resume not found");
            return Result<Resume>.Success(resume);
        }
    }
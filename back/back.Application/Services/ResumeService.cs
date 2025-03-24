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
            if (resume == null)
                return Result.Failure("Resume not found");
            if (resume.UserId != userId)
                return Result.Failure("Forbidden");

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
            if (resume == null)
                return Result.Failure("Resume not found");
            if (resume.UserId != userId)
                return Result.Failure("Forbidden");

            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
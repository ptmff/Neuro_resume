using back.Application.Common;
using back.Application.DTOs;
using back.Domain.Entities;

namespace back.Application.Interfaces;

public interface IResumeService
{
    Task<Result<IEnumerable<Resume>>> GetUserResumes(int userId);
    Task<Result<Resume>> CreateResume(int userId, ResumeDto dto);
    Task<Result> UpdateResume(int userId, int resumeId, ResumeDto dto);
    Task<Result> DeleteResume(int userId, int resumeId);
}
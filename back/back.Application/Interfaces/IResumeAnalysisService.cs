namespace back.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTOs;
using Common;

public interface IResumeAnalysisService
{
    Task<Result<List<SuggestionDto>>> AnalyzeResumeAsync(ResumeDtoForAi resume);
    Task<Result<VacancyAnalysisResultDto>> AnalyzeResumeVacancyAsync(ResumeVacancyAnalysisRequestDto request);
}
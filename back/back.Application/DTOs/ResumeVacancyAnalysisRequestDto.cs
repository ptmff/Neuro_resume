namespace back.Application.DTOs;

// Входной запрос для анализа резюме с вакансией.
public class ResumeVacancyAnalysisRequestDto
{
    public ResumeDtoForAi Resume { get; set; }
    public string VacancyUrl { get; set; }
}
    
// DTO, описывающее итоговый результат анализа вакансии и резюме.
public class VacancyAnalysisResultDto
{
    public int MatchScore { get; set; }
    public List<string> MissingSkills { get; set; }
    public bool OverlappingExperience { get; set; }
    public List<string> Recommendations { get; set; }
}
namespace back.Application.DTOs;

public class ResumeDto
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Job { get; set; }
    public List<string> Skills { get; set; }
    public string City { get; set; }
    public List<ExperienceDto> Experience { get; set; } = new List<ExperienceDto>();
    public string Template { get; set; }
    public string Description { get; set; }
}

public class ExperienceDto
{
    public string Company { get; set; }
    public string Position { get; set; }
    // Даты в формате "YYYY-MM"
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Description { get; set; }
}

public class ResumeDtoForAi
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Job { get; set; }
    public List<string> Skills { get; set; }
    public string City { get; set; }
    public List<ExperienceDto> Experience { get; set; } = new List<ExperienceDto>();
    public List<EducationDto> Education { get; set; } = new List<EducationDto>();
    public string Template { get; set; }
    public string Description { get; set; }
}


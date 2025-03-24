namespace back.Application.DTOs;

public class ResumeDto
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Job { get; set; }
    public List<string> Skills { get; set; }
}
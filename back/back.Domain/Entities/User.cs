namespace back.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Name { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public List<Education>? Education { get; set; } = new List<Education>();
    public int? MainResumeId { get; set; } 
    public string? Photo { get; set; }  

    // Связь с резюме
    public List<Resume> Resumes { get; set; } = new List<Resume>();
}

public class Education
{
    public string Institution { get; set; }
    public string Degree { get; set; }
    public string Field { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
}
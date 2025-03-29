namespace back.Application.DTOs;

public class EducationDto
{
    public string Institution { get; set; }
    public string Degree { get; set; }
    public string Field { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
}

public class UserDto
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public List<EducationDto>? Education { get; set; } = new List<EducationDto>();
    public int? MainResumeId { get; set; }
    public string? Photo { get; set; }
}

public class UserRegisterDto
{
    public string? Email { get; set; }
    public string? Phone { get; set; }  // новый параметр для регистрации по номеру
    public string Password { get; set; }
    public string? Name { get; set; }
    public string? City { get; set; }
}

public class UserLoginDto
{
    public string? Email { get; set; }
    public string? Phone { get; set; }  // новый параметр для логина по номеру
    public string Password { get; set; }
}

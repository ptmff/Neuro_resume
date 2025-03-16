using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back.Models;

public class Education
{
    public string Institution { get; set; }
    public string Degree { get; set; }
    public string Field { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }

    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    // Новые поля
    public string Name { get; set; }
    public string Phone { get; set; }
    public string City { get; set; }
    public string Photo { get; set; }

    [Column(TypeName = "jsonb")]
    public List<Education>? Education { get; set; }


    public int? MainResumeId { get; set; }

    public List<Resume> Resumes { get; set; }
}

public class UserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserRegisterDto : UserDto { }

public class UserLoginDto : UserDto { }

public class UserProfileDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string City { get; set; }
    public string Photo { get; set; }
    public List<Education> Education { get; set; }
    public int? MainResumeId { get; set; }
    public List<int> Resumes { get; set; }
}

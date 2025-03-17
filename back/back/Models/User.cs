using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string City { get; set; }
    public string Photo { get; set; }
    public int? MainResumeId { get; set; }

    public ICollection<Resume> Resumes { get; set; } = new List<Resume>();
    public ICollection<Education> Education { get; set; } = new List<Education>();
}

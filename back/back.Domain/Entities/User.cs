namespace back.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    // Связь с резюме
    public List<Resume> Resumes { get; set; } = new List<Resume>();
}
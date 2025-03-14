using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace back.Models;

[Owned]
public class Resume
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Job { get; set; }
    
    [Column(TypeName = "jsonb")]
    public List<string> Skills { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
}

public class ResumeDto
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Job { get; set; }
    public List<string> Skills { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace back.Models;

[Owned]
public class Experience
{
    public string Company { get; set; }
    public string Position { get; set; }
    public string StartDate { get; set; }  // храним как строку, если формат ISO "YYYY-MM"
    public string EndDate { get; set; }
    public string Description { get; set; }
}

public class Resume
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Job { get; set; }
    public string City { get; set; }
    public string Template { get; set; }

    [Column(TypeName = "jsonb")]
    public List<string> Skills { get; set; }

    [Column(TypeName = "jsonb")]
    public List<Experience> Experience { get; set; }

    [JsonIgnore]
    [ForeignKey("UserId")]
    public User User { get; set; }
}

public class ResumeDto
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Job { get; set; }
    public string City { get; set; }
    public string Template { get; set; }
    public List<string> Skills { get; set; }
    public List<Experience> Experience { get; set; }
}

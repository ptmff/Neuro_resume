using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace back.Domain.Entities;

[Owned]
public class Resume
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Job { get; set; }
    public List<string> Skills { get; set; }
    public string City { get; set; }
    public List<Experience> Experience { get; set; } = new List<Experience>();
    public string Template { get; set; }
    public string Description { get; set; }

    [JsonIgnore]
    [ForeignKey("UserId")]
    public User User { get; set; }
}

public class Experience
{
    public string Company { get; set; }
    public string Position { get; set; }
    // Даты записываем в формате "YYYY-MM". Можно использовать строку или создать конвертер для DateTime.
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Description { get; set; }
}
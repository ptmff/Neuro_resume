using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace back.Models;

[Owned]
public class Resume
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string City { get; set; }
    public string Job { get; set; }
    public string Template { get; set; }

    public List<string> Skills { get; set; } // JSONB поле или сериализация

    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<Experience> Experience { get; set; } = new List<Experience>();
}

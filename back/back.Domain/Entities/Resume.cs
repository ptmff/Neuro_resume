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

    [JsonIgnore]
    [ForeignKey("UserId")]
    public User User { get; set; }
}
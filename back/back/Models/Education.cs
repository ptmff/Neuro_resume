using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace back.Models;

[Owned]
public class Education
{
    public int Id { get; set; }
    public string Institution { get; set; }
    public string Degree { get; set; }
    public string Field { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}

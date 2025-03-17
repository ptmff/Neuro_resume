using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using back.Models;

namespace back.Models;

public class Experience
{
    public int Id { get; set; }
    public string Company { get; set; }
    public string Position { get; set; }
    public string StartDate { get; set; }  // <-- string
    public string EndDate { get; set; }    // <-- string
    public string Description { get; set; }

    public int ResumeId { get; set; }
    public Resume Resume { get; set; }
}

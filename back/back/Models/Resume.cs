namespace back.Models;

public class Resume{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string job { get; set; }
    public List<string> skills { get; set; }
}
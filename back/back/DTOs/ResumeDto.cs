namespace back.DTOs
{
    public class ResumeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Job { get; set; }
        public string Template { get; set; }
        public List<string> Skills { get; set; }
        public List<ExperienceDto> Experience { get; set; }
    }
}
namespace back.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Photo { get; set; }
        public int? MainResumeId { get; set; }
        public List<int> Resumes { get; set; }
        public List<EducationDto> Education { get; set; }
    }
}
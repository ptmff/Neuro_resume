namespace back.DTOs
{
    public class ProfileUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Photo { get; set; }
        public int? MainResumeId { get; set; }
        public List<EducationDto> Education { get; set; }
        public List<int> Resumes { get; set; } // опционально
    }
}
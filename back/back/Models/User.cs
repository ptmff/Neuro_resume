namespace back.Models;

public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        // Навигационное свойство для связи с резюме
        public List<Resume> Resumes { get; set; } // Связь с резюме
    }

    public class UserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserRegisterDto : UserDto { }

    public class UserLoginDto : UserDto { }
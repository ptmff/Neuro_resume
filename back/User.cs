// Models/User.cs
using System.ComponentModel.DataAnnotations;

namespace AuthExample.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
    }

    public class RegisterRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
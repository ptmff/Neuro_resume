namespace back.Application.DTOs;

public class UserDto
{
    public string Email { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
}

public class UserRegisterDto : UserDto
{
    public string Password { get; set; }
}

public class UserLoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
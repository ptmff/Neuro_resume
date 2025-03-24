namespace back.Application.DTOs;

public class UserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

// DTO для регистрации пользователя
public class UserRegisterDto : UserDto
{
    // Можно добавить дополнительные поля, если потребуется
}

// DTO для аутентификации пользователя
public class UserLoginDto : UserDto
{
    // Можно добавить дополнительные поля, если потребуется
}
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using back.Data;
using System.Text;
using back.Models;

namespace back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserRegisterDto request)
    {
        var result = await _authService.RegisterUserAsync(request);
        return result.Match<ActionResult>(
            user => Ok(user),
            error => BadRequest(error)
        );
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserLoginDto request)
    {
        var result = await _authService.LoginUserAsync(request);
        return result.Match<ActionResult>(
            token => Ok(token),
            error => BadRequest(error)
        );
    }
}

// Новые сервисы и интерфейсы
public interface IPasswordService
{
    void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);
    bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
}

public interface ITokenService
{
    string CreateToken(User user);
}

public interface IAuthService
{
    Task<Result<User>> RegisterUserAsync(UserRegisterDto request);
    Task<Result<string>> LoginUserAsync(UserLoginDto request);
}

public class PasswordService : IPasswordService
{
    public void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(_configuration.GetValue<double>("Jwt:ExpireDays")),
            SigningCredentials = creds,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public AuthService(
        AppDbContext context,
        IPasswordService passwordService,
        ITokenService tokenService)
    {
        _context = context;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<Result<User>> RegisterUserAsync(UserRegisterDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return Result<User>.Failure("User already exists");

        _passwordService.CreatePasswordHash(request.Password, 
            out byte[] hash, out byte[] salt);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Result<User>.Success(user);
    }

    public async Task<Result<string>> LoginUserAsync(UserLoginDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !_passwordService.VerifyPasswordHash(
            request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Result<string>.Failure("Invalid credentials");
        }

        return Result<string>.Success(_tokenService.CreateToken(user));
    }
}

// Вспомогательный класс для обработки результатов
// public class Result<T>
// {
//     public T Value { get; }
//     public string Error { get; }
//     public bool IsSuccess { get; }

//     private Result(T value) { Value = value; IsSuccess = true; }
//     private Result(string error) { Error = error; IsSuccess = false; }

//     public static Result<T> Success(T value) => new Result<T>(value);
//     public static Result<T> Failure(string error) => new Result<T>(error);

//     public TResult Match<TResult>(Func<T, TResult> success, Func<string, TResult> failure)
//         => IsSuccess ? success(Value!) : failure(Error);
// }

// Необобщенный класс Result для операций без возвращаемого значения
public class Result
{
    public string Error { get; }
    public bool IsSuccess { get; }

    private Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, null);
    public static Result Failure(string error) => new Result(false, error);

    public TResult Match<TResult>(Func<TResult> success, Func<string, TResult> failure)
        => IsSuccess ? success() : failure(Error);
}

// Обобщенная версия Result<T>
public class Result<T>
{
    public T Value { get; }
    public string Error { get; }
    public bool IsSuccess { get; }

    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }

    private Result(string error)
    {
        Error = error;
        IsSuccess = false;
    }

    public static Result<T> Success(T value) => new Result<T>(value);
    public static Result<T> Failure(string error) => new Result<T>(error);

    public TResult Match<TResult>(Func<T, TResult> success, Func<string, TResult> failure)
        => IsSuccess ? success(Value!) : failure(Error);
}
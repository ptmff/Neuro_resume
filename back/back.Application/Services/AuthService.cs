using back.Application.Common;
using back.Application.DTOs;
using back.Application.Interfaces;
using back.Domain.Entities;
using back.Infrastructure.Persistence;
using back.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace back.Application.Services;

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

    public async Task<Result<UserDto>> RegisterUserAsync(UserRegisterDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Phone))
        {
            return Result<UserDto>.Failure("Необходимо указать Email или Phone");
        }

        if (!string.IsNullOrWhiteSpace(request.Email) &&
            await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return Result<UserDto>.Failure("Пользователь с таким email уже существует");
        }

        if (!string.IsNullOrWhiteSpace(request.Phone) &&
            await _context.Users.AnyAsync(u => u.Phone == request.Phone))
        {
            return Result<UserDto>.Failure("Пользователь с таким номером телефона уже существует");
        }

        _passwordService.CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

        var user = new User
        {
            Email = request.Email,
            Phone = request.Phone,
            PasswordHash = hash,
            PasswordSalt = salt,
            Name = request.Name,
            City = request.City
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var resultDto = new UserDto
        {
            Email = user.Email,
            Phone = user.Phone,
            Name = user.Name,
            City = user.City
        };

        return Result<UserDto>.Success(resultDto);
    }

    public async Task<Result<string>> LoginUserAsync(UserLoginDto request)
    {
        // Проверяем, что передан хотя бы email или phone
        if (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.Phone))
        {
            return Result<string>.Failure("Необходимо указать Email или Phone для авторизации");
        }

        User user = null;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        }
        else if (!string.IsNullOrWhiteSpace(request.Phone))
        {
            user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.Phone);
        }

        if (user == null || !_passwordService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Result<string>.Failure("Неверные учётные данные");
        }

        var token = _tokenService.CreateToken(user);
        return Result<string>.Success(token);
    }
}

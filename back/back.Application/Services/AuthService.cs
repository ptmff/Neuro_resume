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
            // Проверка на существование пользователя с таким email
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return Result<UserDto>.Failure("User already exists");

            _passwordService.CreatePasswordHash(request.Password, out byte[] hash, out byte[] salt);

            // Создаём доменную модель User
            var user = new User
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Name = request.Name,         // новое поле
                Phone = request.Phone,       // новое поле (optional)
                City = request.City          // новое поле (optional)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var resultDto = new UserDto
            {
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                City = user.City
            };

            return Result<UserDto>.Success(resultDto);
        }

        public async Task<Result<string>> LoginUserAsync(UserLoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !_passwordService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Result<string>.Failure("Invalid credentials");
            }

            var token = _tokenService.CreateToken(user);
            return Result<string>.Success(token);
        }
    }
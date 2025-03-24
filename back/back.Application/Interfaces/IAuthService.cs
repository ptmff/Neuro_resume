using back.Application.Common;
using back.Application.DTOs;

namespace back.Application.Interfaces;

public interface IAuthService
{
    Task<Result<UserDto>> RegisterUserAsync(UserRegisterDto request);
    Task<Result<string>> LoginUserAsync(UserLoginDto request);
}
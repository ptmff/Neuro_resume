using back.Application.Common;
using back.Application.DTOs;

namespace back.Application.Interfaces;

public interface IProfileService
{
    Task<Result<UserDto>> GetProfileAsync(int userId);
    Task<Result<UserDto>> UpdateProfileAsync(int userId, UserDto updateData);
    Task<Result> DeleteProfileAsync(int userId);
}

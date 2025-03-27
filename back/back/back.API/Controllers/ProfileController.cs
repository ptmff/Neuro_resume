using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using back.Application.Common;
using back.Application.DTOs;
using back.Application.Interfaces;
using back.API.Services;

namespace back.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IUserContext _userContext;

    public ProfileController(IProfileService profileService, IUserContext userContext)
    {
        _profileService = profileService;
        _userContext = userContext;
    }

    // GET /api/profile
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var result = await _profileService.GetProfileAsync(_userContext.UserId);
        return result.ToActionResult();
    }

    // PATCH /api/profile
    [HttpPatch]
    public async Task<IActionResult> UpdateProfile([FromBody] UserDto updateData)
    {
        var result = await _profileService.UpdateProfileAsync(_userContext.UserId, updateData);
        return result.ToActionResult();
    }

    // DELETE /api/profile
    [HttpDelete]
    public async Task<IActionResult> DeleteProfile()
    {
        var result = await _profileService.DeleteProfileAsync(_userContext.UserId);
        return result.ToActionResult();
    }
}
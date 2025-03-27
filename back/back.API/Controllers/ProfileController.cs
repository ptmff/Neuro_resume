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
    private readonly IWebHostEnvironment _env;

    public ProfileController(IProfileService profileService, IUserContext userContext, IWebHostEnvironment env)
    {
        _profileService = profileService;
        _userContext = userContext;
        _env = env;
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
    
    // POST /api/profile/photo
    [HttpPost("photo")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadPhoto([FromForm] FileUploadDto uploadDto)
    {
        var file = uploadDto.File;
        if (file == null || file.Length == 0)
            return BadRequest("Файл не выбран");

        // Генерируем уникальное имя файла
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var uploadPath = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Формируем URL для доступа к файлу
        var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

        // Обновляем профиль: сохраняем URL фото
        var updateResult = await _profileService.UpdateProfileAsync(_userContext.UserId,
            new UserDto { Photo = fileUrl });
        if (!updateResult.IsSuccess)
            return BadRequest(updateResult.Error);

        return Ok(new { PhotoUrl = fileUrl });
    }


    // DELETE /api/profile
    [HttpDelete]
    public async Task<IActionResult> DeleteProfile()
    {
        var result = await _profileService.DeleteProfileAsync(_userContext.UserId);
        return result.ToActionResult();
    }
}
using Microsoft.AspNetCore.Mvc;
using back.Application.DTOs;            // DTO для регистрации и логина
using back.Application.Interfaces;        // Интерфейс IAuthService

using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace back.API.Controllers;

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
    public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
    {
        var result = await _authService.RegisterUserAsync(request);
        return result.Match<IActionResult>(
            user => Ok(user),
            error => BadRequest(error)
        );

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto request)
    {
        var result = await _authService.LoginUserAsync(request);
        return result.Match<IActionResult>(
            user => Ok(user),
            error => BadRequest(error)
        );

    }
}
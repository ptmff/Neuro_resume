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
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserRegisterDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest("User already exists");

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserLoginDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Invalid credentials");

        var token = CreateToken(user);
        return Ok(token);
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }

    private string CreateToken(User user)
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

// [ApiController]
// [Route("api/[controller]")]
// [Authorize]
// public class ResumesController : ControllerBase
// {
//     private readonly AppDbContext _context;

//     public ResumesController(AppDbContext context)
//     {
//         _context = context;
//     }

//     // GET: api/resumes
//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<Resume>>> GetResumes()
//     {
//         var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
//         return await _context.Resumes
//             .Where(r => r.UserId == userId)
//             .ToListAsync();
//     }

//     // POST: api/resumes
//     [HttpPost]
//     public async Task<ActionResult<Resume>> PostResume(Resume resume)
//     {
//         var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
//         resume.UserId = userId;
        
//         _context.Resumes.Add(resume);
//         await _context.SaveChangesAsync();

//         return CreatedAtAction(nameof(GetResumes), new { id = resume.Id }, resume);
//     }

//     // PUT: api/resumes/5
//     [HttpPut("{id}")]
//     public async Task<IActionResult> PutResume(int id, Resume resume)
//     {
//         var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
//         if (id != resume.Id)
//         {
//             return BadRequest();
//         }

//         var existingResume = await _context.Resumes.FindAsync(id);
//         if (existingResume == null)
//         {
//             return NotFound();
//         }

//         if (existingResume.UserId != userId)
//         {
//             return Unauthorized();
//         }

//         existingResume.Title = resume.Title;
//         existingResume.Date = resume.Date;
//         existingResume.Job = resume.Job;
//         existingResume.Skills = resume.Skills;

//         _context.Entry(existingResume).State = EntityState.Modified;

//         try
//         {
//             await _context.SaveChangesAsync();
//         }
//         catch (DbUpdateConcurrencyException)
//         {
//             if (!ResumeExists(id))
//             {
//                 return NotFound();
//             }
//             else
//             {
//                 throw;
//             }
//         }

//         return NoContent();
//     }

//     // DELETE: api/resumes/5
//     [HttpDelete("{id}")]
//     public async Task<IActionResult> DeleteResume(int id)
//     {
//         var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
//         var resume = await _context.Resumes.FindAsync(id);
        
//         if (resume == null)
//         {
//             return NotFound();
//         }

//         if (resume.UserId != userId)
//         {
//             return Unauthorized();
//         }

//         _context.Resumes.Remove(resume);
//         await _context.SaveChangesAsync();

//         return NoContent();
//     }

//     private bool ResumeExists(int id)
//     {
//         return _context.Resumes.Any(e => e.Id == id);
//     }
// }
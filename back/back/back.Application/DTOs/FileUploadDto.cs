using Microsoft.AspNetCore.Http;

namespace back.Application.DTOs;

public class FileUploadDto
{
    public IFormFile File { get; set; }
}
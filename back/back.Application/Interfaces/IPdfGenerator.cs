using back.Application.Common;
using back.Application.DTOs;

namespace back.Application.Interfaces;

public interface IPdfGenerator
{
    Task<byte[]> GeneratePdfAsync(string htmlContent);
}
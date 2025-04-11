using back.Application.Common;
using back.Application.DTOs;

namespace back.Application.Interfaces;

public interface IHtmlGenerator
{
    string Generate(ResumeDto resumeDto);
}
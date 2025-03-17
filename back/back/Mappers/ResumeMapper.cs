using back.Models;
using back.DTOs;
using back.Mappers;

namespace back.Mappers;

public static class ResumeMapper
{
    public static Resume ToModel(this ResumeDto dto) =>
        new Resume {
            Title = dto.Title,
            Date = dto.Date,
            Job = dto.Job,
            City = dto.City,
            Template = dto.Template,
            Skills = dto.Skills,
            Experience = dto.Experience?.Select(e => e.ToModel()).ToList()
        };

    public static ResumeDto ToDto(this Resume model) =>
        new ResumeDto {
            Id = model.Id,
            Title = model.Title,
            Date = model.Date,
            Job = model.Job,
            City = model.City,
            Template = model.Template,
            Skills = model.Skills,
            Experience = model.Experience?.Select(e => e.ToDto()).ToList()
        };
}

using back.Models;
using back.DTOs;

namespace back.Mappers;

public static class ExperienceMapper
{
    public static Experience ToModel(this ExperienceDto dto)
    {
        return new Experience
        {
            Company = dto.Company,
            Position = dto.Position,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Description = dto.Description
        };
    }

    public static ExperienceDto ToDto(this Experience model)
    {
        return new ExperienceDto
        {
            Company = model.Company,
            Position = model.Position,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Description = model.Description
        };
    }
}

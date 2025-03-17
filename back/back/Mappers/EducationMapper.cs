using back.Models;
using back.DTOs;

namespace back.Mappers;

public static class EducationMapper
{
    public static EducationDto ToDto(this Education model)
    {
        return new EducationDto
        {
            Institution = model.Institution,
            Degree = model.Degree,
            Field = model.Field,
            StartYear = model.StartYear,
            EndYear = model.EndYear
        };
    }

    public static Education ToModel(this EducationDto dto)
    {
        return new Education
        {
            Institution = dto.Institution,
            Degree = dto.Degree,
            Field = dto.Field,
            StartYear = dto.StartYear,
            EndYear = dto.EndYear
        };
    }
}

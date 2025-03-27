using Microsoft.EntityFrameworkCore;
using back.Application.Common;
using back.Application.DTOs;
using back.Application.Interfaces;
using back.Domain.Entities;
using back.Infrastructure.Persistence;

namespace back.Application.Services;

public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;

        public ProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<UserDto>> GetProfileAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Result<UserDto>.Failure("User not found");

            var dto = new UserDto
            {
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                City = user.City,
                Education = user.Education?.Select(e => new EducationDto
                {
                    Institution = e.Institution,
                    Degree = e.Degree,
                    Field = e.Field,
                    StartYear = e.StartYear,
                    EndYear = e.EndYear
                }).ToList(),
                MainResumeId = user.MainResumeId,
                Photo = user.Photo
            };

            return Result<UserDto>.Success(dto);
        }

        public async Task<Result<UserDto>> UpdateProfileAsync(int userId, UserDto updateData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Result<UserDto>.Failure("User not found");

            // Обновляем только те поля, которые переданы в updateData
            if (!string.IsNullOrEmpty(updateData.Name))
                user.Name = updateData.Name;
            if (!string.IsNullOrEmpty(updateData.Phone))
                user.Phone = updateData.Phone;
            if (!string.IsNullOrEmpty(updateData.City))
                user.City = updateData.City;
            if (updateData.Education != null)
                user.Education = updateData.Education.Select(e => new Education
                {
                    Institution = e.Institution,
                    Degree = e.Degree,
                    Field = e.Field,
                    StartYear = e.StartYear,
                    EndYear = e.EndYear
                }).ToList();
            if (updateData.MainResumeId.HasValue)
                user.MainResumeId = updateData.MainResumeId;
            if (!string.IsNullOrEmpty(updateData.Photo))
                user.Photo = updateData.Photo;

            await _context.SaveChangesAsync();

            var dto = new UserDto
            {
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                City = user.City,
                Education = user.Education?.Select(e => new EducationDto
                {
                    Institution = e.Institution,
                    Degree = e.Degree,
                    Field = e.Field,
                    StartYear = e.StartYear,
                    EndYear = e.EndYear
                }).ToList(),
                MainResumeId = user.MainResumeId,
                Photo = user.Photo
            };

            return Result<UserDto>.Success(dto);
        }

        public async Task<Result> DeleteProfileAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Result.Failure("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
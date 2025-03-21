﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using back.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace back.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Resume> Resumes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи один-ко-многим
            modelBuilder.Entity<User>()
                .HasMany(u => u.Resumes)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);

            // Настройка типа данных для поля Skills
            modelBuilder.Entity<Resume>()
                .Property(r => r.Skills)
                .HasColumnType("jsonb");

            modelBuilder.Entity<Resume>()
        .Property(r => r.Skills)
        .HasColumnType("jsonb")
        .HasConversion(
            new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions())!));
        }
    }
}
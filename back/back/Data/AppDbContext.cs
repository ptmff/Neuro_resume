using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using back.Models;

namespace back.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Resume> Resumes { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    // Настройка связи один-ко-многим
    //    modelBuilder.Entity<User>()
    //        .HasMany(u => u.Resumes)
    //        .WithOne(r => r.User)
    //        .HasForeignKey(r => r.UserId);
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Resume>()
            .Property(r => r.Skills)
            .HasColumnType("jsonb");
    }
}
using System.Text.Json;
using back.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace back.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Resume> Resumes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .Property(u => u.Name)
            .IsRequired(); // Name обязательное
        
        // Для Phone и City можно оставить как опциональные:
        modelBuilder.Entity<User>()
            .Property(u => u.Phone)
            .IsRequired(false);

        modelBuilder.Entity<User>()
            .Property(u => u.City)
            .IsRequired(false);

        // Связь "один-ко-многим": User - Resumes
        modelBuilder.Entity<User>()
            .HasMany(u => u.Resumes)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        // Настройка хранения списка навыков в формате JSON (PostgreSQL jsonb)
        modelBuilder.Entity<Resume>()
            .Property(r => r.Skills)
            .HasColumnType("jsonb")
            .HasConversion(
                new ValueConverter<List<string>, string>(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions())!));
    }
}
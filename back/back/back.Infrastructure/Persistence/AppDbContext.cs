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

        // Конфигурация User
        modelBuilder.Entity<User>()
            .Property(u => u.Name)
            .IsRequired(); // Name обязательное
            
        modelBuilder.Entity<User>()
            .Property(u => u.Phone)
            .IsRequired(false);

        modelBuilder.Entity<User>()
            .Property(u => u.City)
            .IsRequired(false);
        
        // Настройка хранения образования как JSON:
        modelBuilder.Entity<User>()
            .Property(u => u.Education)
            .HasColumnType("jsonb")
            .HasConversion(
                new ValueConverter<List<Education>, string>(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<Education>>(v, new JsonSerializerOptions())!));

        // Новые поля MainResumeId и Photo можно настроить как текст/целое число:
        modelBuilder.Entity<User>()
            .Property(u => u.Photo)
            .HasColumnType("text");

        modelBuilder.Entity<User>()
            .HasMany(u => u.Resumes)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId);

        // Конфигурация Resume
        modelBuilder.Entity<Resume>()
            .Property(r => r.Skills)
            .HasColumnType("jsonb")
            .HasConversion(
                new ValueConverter<List<string>, string>(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions())!));

        // ✅ Добавляем поддержку сериализации Experience в JSON
        modelBuilder.Entity<Resume>()
            .Property(r => r.Experience)
            .HasColumnType("jsonb")
            .HasConversion(
                new ValueConverter<List<Experience>, string>(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<List<Experience>>(v, new JsonSerializerOptions())!));

        // Новые свойства Resume
        modelBuilder.Entity<Resume>()
            .Property(r => r.City)
            .HasColumnType("text");

        modelBuilder.Entity<Resume>()
            .Property(r => r.Template)
            .HasColumnType("text");

        modelBuilder.Entity<Resume>()
            .Property(r => r.Description)
            .HasColumnType("text");
    }
}
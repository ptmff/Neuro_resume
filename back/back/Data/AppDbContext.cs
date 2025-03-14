using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using back.Models;

namespace back.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
using Microsoft.EntityFrameworkCore;
using CheckPoint2App.Models;

namespace CheckPoint2App.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!; // <-- Adds the missing Users table definition
}
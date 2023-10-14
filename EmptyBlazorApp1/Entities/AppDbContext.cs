using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EmptyBlazorApp1.Entities;

public class AppDbContext : DbContext {
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;

    public AppDbContext() {
        Console.WriteLine("AppDbContext ctor");
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { 
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}

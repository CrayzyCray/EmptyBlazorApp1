using Microsoft.EntityFrameworkCore;

namespace EmptyBlazorApp1.Entities;

public class AppDbContext : DbContext {
    public DbSet<User>              Users       { get; set; } = null!;
    public DbSet<UserProfile>       UserProfile { get; set; } = null!;
    public DbSet<Session>           Sessions    { get; set; } = null!;
    public DbSet<Community>         Communities { get; set; } = null!;
    public DbSet<CommunityHashTags> HashTags    { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder mBuilder) {
        mBuilder.Entity<User>()
                .HasOne(u => u.UserProfile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        
        mBuilder.Entity<User>()
                .HasMany(u => u.Communities)
                .WithMany(c => c.Members);


        mBuilder.Entity<Community>()
                .HasOne(c => c.Creator)
                .WithMany(u => u.CreatedCommunities)
                .HasForeignKey(a => a.CreatorId);

        mBuilder.Entity<Community>()
                .HasMany(c => c.HashTags);
    }
}
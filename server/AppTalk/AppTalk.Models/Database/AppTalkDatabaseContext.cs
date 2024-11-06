using AppTalk.Models.Models;
using AppTalk.Utils.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AppTalk.Models.Database;

public class AppTalkDatabaseContext(DbContextOptions<AppTalkDatabaseContext> options) : DbContext(options)
{
    private const string SchemaName = "app_talk";

    public DbSet<User> Users { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<ServerMember> ServerMembers { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Message> Messages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        modelBuilder.SetDefaultValues();

        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();

        modelBuilder.Entity<ServerMember>().HasIndex(x => new { x.UserId, x.ServerId }).IsUnique();
    }
}
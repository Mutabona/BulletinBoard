using BulletinBoard.DataAccess.Configurations;
using BulletinBoard.Domain.Bulletins.Entity;
using BulletinBoard.Domain.Categories.Entity;
using BulletinBoard.Domain.Files.Images.Entity;
using BulletinBoard.Domain.Users.Entity;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DataAccess;

public class ApplicationDbContext : DbContext
{
    
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BulletinConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ImageConfiguration());
    }
}
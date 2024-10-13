using BulletinBoard.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DataAccess;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Создаёт экземпляр <see cref="ApplicationDbContext"/>.
    /// </summary>
    /// <param name="options">Опции.</param>
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BulletinConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ImageConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
    }
}
using BulletinBoard.Domain.Bulletins.Entity;
using BulletinBoard.Domain.Users.Entity;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DataAccess;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Bulletin> Bulletins { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}
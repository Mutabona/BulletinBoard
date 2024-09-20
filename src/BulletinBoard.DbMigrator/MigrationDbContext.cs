using BulletinBoard.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DbMigrator;

public class MigrationDbContext : ApplicationDbContext
{
    /// <summary>
    /// Создаёт экземпляр <see cref="MigrationDbContext"/>
    /// </summary>
    /// <param name="dbContextOptions"></param>
    public MigrationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {

    }
}
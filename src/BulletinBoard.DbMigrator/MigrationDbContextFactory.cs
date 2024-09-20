using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BulletinBoard.DbMigrator;

public class MigrationDbContextFactory : IDesignTimeDbContextFactory<MigrationDbContext>
{
    public MigrationDbContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = builder.Build();
        var connectionString = configuration.GetConnectionString("DbConnection");

        var dbContextOptionsBuilder = new DbContextOptionsBuilder<MigrationDbContext>();
        dbContextOptionsBuilder.UseNpgsql(connectionString);
        return new MigrationDbContext(dbContextOptionsBuilder.Options);
    }
}
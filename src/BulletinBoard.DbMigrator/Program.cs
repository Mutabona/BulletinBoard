// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BulletinBoard.DbMigrator;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
        {
            services.AddServices(hostContext.Configuration);
        }).Build();
        
        await MigrateAsync(host.Services);
    }

    private static async Task MigrateAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MigrationDbContext>();
        await context.Database.MigrateAsync();
    }
}
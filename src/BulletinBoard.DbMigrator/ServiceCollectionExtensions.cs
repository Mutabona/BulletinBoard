using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BulletinBoard.DbMigrator;

/// <summary>
/// Расширение коллекции сервисов.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет сервисы.
    /// </summary>
    /// <param name="services">Коллекция сервисов <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">Конфигурация <see cref="IConfiguration"/>.</param>
    /// <returns>Сервисы <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) 
    {
        services.ConfigureDbConnections(configuration);
        return services;
    }

    /// <summary>
    /// Конфигурирует подключения базы данных.
    /// </summary>
    /// <param name="services">Коллекция сервисов <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">Конфигурация <see cref="IConfiguration"/>.</param>
    /// <returns>Сервисы <see cref="IServiceCollection"/>.</returns>
    private static IServiceCollection ConfigureDbConnections(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        services.AddDbContext<MigrationDbContext>(options => options.UseNpgsql(connectionString));
        return services;
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace BulletinBoard.ComponentRegistrar;

public static class Registrar
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}
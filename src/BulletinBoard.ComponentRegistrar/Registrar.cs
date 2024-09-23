using AutoMapper;
using BulletinBoard.AppServices.Contexts.Bulletins.Builders;
using BulletinBoard.AppServices.Contexts.Bulletins.Repositories;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Categories.Repositories;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.AppServices.Contexts.Files.Images.Repositories;
using BulletinBoard.AppServices.Contexts.Files.Images.Services;
using BulletinBoard.AppServices.Contexts.Users.Repositories;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.DataAccess.Bulletins.Repository;
using BulletinBoard.DataAccess.Categories.Repository;
using BulletinBoard.DataAccess.Files.Images.Repository;
using BulletinBoard.DataAccess.Users.Repository;
using BulletinBoard.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BulletinBoard.ComponentRegistrar;

public static class Registrar
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService> ();
        services.AddScoped<IBulletinService, BulletinService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IImageService, ImageService>();
        
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IBulletinRepository, BulletinRepository>();
        services.AddSingleton<ICategoryRepository, CategoryRepository>();
        services.AddSingleton<IImageRepository, ImageRepository>();

        services.AddScoped<IBulletinSpecificationBuilder, BulletinSpecificationBuilder>();
        
        services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));
        
        return services;
    }

    private static MapperConfiguration GetMapperConfiguration()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BulletinProfile>();
            cfg.AddProfile<ImageProfile>();
            cfg.AddProfile<CategoryProfile>();
            cfg.AddProfile<UserProfile>();
        });
        
        configuration.AssertConfigurationIsValid();
        return configuration;
    }
}
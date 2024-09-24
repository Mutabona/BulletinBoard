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
using BulletinBoard.DataAccess;
using BulletinBoard.DataAccess.Bulletins.Repository;
using BulletinBoard.DataAccess.Categories.Repository;
using BulletinBoard.DataAccess.Files.Images.Repository;
using BulletinBoard.DataAccess.Users.Repository;
using BulletinBoard.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BulletinBoard.ComponentRegistrar;

public static class Registrar
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IJwtService, JwtService> ();
        services.AddTransient<IBulletinService, BulletinService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IImageService, ImageService>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBulletinRepository, BulletinRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();

        services.AddScoped<IBulletinSpecificationBuilder, BulletinSpecificationBuilder>();
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));
        
        services.AddScoped<DbContext>(s => s.GetRequiredService<ApplicationDbContext>());
        
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
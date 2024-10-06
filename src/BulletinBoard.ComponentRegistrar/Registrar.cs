using AutoMapper;
using BulletinBoard.AppServices.Contexts.Bulletins.Builders;
using BulletinBoard.AppServices.Contexts.Bulletins.Repositories;
using BulletinBoard.AppServices.Contexts.Bulletins.Services;
using BulletinBoard.AppServices.Contexts.Categories.Repositories;
using BulletinBoard.AppServices.Contexts.Categories.Services;
using BulletinBoard.AppServices.Contexts.Comments.Repositories;
using BulletinBoard.AppServices.Contexts.Comments.Services;
using BulletinBoard.AppServices.Contexts.Files.Images.Repositories;
using BulletinBoard.AppServices.Contexts.Files.Images.Services;
using BulletinBoard.AppServices.Contexts.Users.Repositories;
using BulletinBoard.AppServices.Contexts.Users.Services;
using BulletinBoard.AppServices.Validators;
using BulletinBoard.AppServices.Validators.Bulletins;
using BulletinBoard.AppServices.Validators.Categories;
using BulletinBoard.AppServices.Validators.Comments;
using BulletinBoard.AppServices.Validators.Users;
using BulletinBoard.ComponentRegistrar.MapProfiles;
using BulletinBoard.DataAccess;
using BulletinBoard.DataAccess.Bulletins.Repository;
using BulletinBoard.DataAccess.Categories.Repository;
using BulletinBoard.DataAccess.Comments.Repository;
using BulletinBoard.DataAccess.Files.Images.Repository;
using BulletinBoard.DataAccess.Users.Repository;
using BulletinBoard.Infrastructure.Repository;
using FluentValidation;
using FluentValidation.AspNetCore;
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
        services.AddTransient<ICommentService, CommentService>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBulletinRepository, BulletinRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        services.AddScoped<IBulletinSpecificationBuilder, BulletinSpecificationBuilder>();
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        services.AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()));
        
        services.AddScoped<DbContext>(s => s.GetRequiredService<ApplicationDbContext>());

        services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateBulletinRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateBulletinValidator>();
        services.AddValidatorsFromAssemblyContaining<AddCommentValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginUserRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
        services.AddFluentValidationAutoValidation();
        
        return services;
    }

    private static MapperConfiguration GetMapperConfiguration()
    {
        TimeProvider timeProvider = TimeProvider.System;
        
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BulletinProfile>();
            cfg.AddProfile<ImageProfile>();
            cfg.AddProfile<CategoryProfile>();
            cfg.AddProfile<UserProfile>();
            cfg.AddProfile<CommentProfile>();
        });
        
        configuration.AssertConfigurationIsValid();
        return configuration;
    }
}
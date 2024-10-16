using System.Text;
using BulletinBoard.API.Controllers;
using BulletinBoard.API.Middlewares;
using BulletinBoard.AppServices.Services;
using BulletinBoard.ComponentRegistrar;
using BulletinBoard.Contracts.Bulletins;
using BulletinBoard.Contracts.Categories;
using BulletinBoard.Contracts.Comments;
using BulletinBoard.Contracts.Files.Images;
using BulletinBoard.Contracts.Users;
using BulletinBoard.DataAccess;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BulletinBoard API", Version = "V1.1" });
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(AccountController).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(BulletinController).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(ImageController).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(CategoryController).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(UserController).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(BulletinDto).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(CategoryDto).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(CreateBulletinRequest).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(UpdateBulletinRequest).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(CreateCategoryRequest).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(LoginUserRequest).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(RegisterUserRequest).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(SearchBulletinRequest).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(ImageDto).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(UserDto).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(AddCommentRequest).Assembly.GetName().Name}.xml")));
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, $"{typeof(CommentDto).Assembly.GetName().Name}.xml")));
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddServices();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = "BulletinBoard_";
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq");
builder.Services.AddMassTransit(mt =>
{
    mt.AddConsumer<NotificationService>();
    mt.UsingRabbitMq((context, cfg) =>
    {
        //cfg.AutoStart = true;
        cfg.Host(rabbitMqSettings.GetValue<string>("Host"), h =>
        {
            h.Username(rabbitMqSettings.GetValue<string>("Username"));
            h.Password(rabbitMqSettings.GetValue<string>("Password"));
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Host.UseSerilog((context, provider, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
        .Enrich.WithEnvironmentName();
});

builder.Services.AddAuthentication(x => 
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

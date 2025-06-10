using MBStream.Data;
using Microsoft.EntityFrameworkCore;
using MBStream.Repositories;
using MBStream.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MBStream.Models;
using MBStream.Mappings;
using Microsoft.OpenApi.Models;
using System;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


        // Add AppDbContext with PostgreSQL connection string
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add authentication and authorization
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
        builder.Services.AddAuthorization();

        // Register repositories and services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
        builder.Services.AddScoped<IAlbumService, AlbumService>();
        builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
        builder.Services.AddScoped<IArtistService, ArtistService>();
        builder.Services.AddScoped<ISongRepository, SongRepository>();
        builder.Services.AddScoped<ISongService, SongService>();
        builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        builder.Services.AddScoped<IPlaylistService, PlaylistService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        // Add controllers and configure AutoMapper
        builder.Services.AddControllers(); // Must be before Swagger configuration
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddEndpointsApiExplorer();

        // Configure Swagger
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });


        var app = builder.Build();

        // Seed admin user
        await SeedAdminUserAsync(app.Services);

        // Configure middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseDefaultFiles();   // <--- Add this line before UseStaticFiles
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static async Task SeedAdminUserAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

        if (await userRepo.GetByEmailAsync("admin@example.com") == null)
        {
            await userRepo.AddAsync(new User
            {
                UserName = "Admin",
                UserEmail = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin"
            });
        }
    }
}

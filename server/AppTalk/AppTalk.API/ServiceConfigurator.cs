using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using AppTalk.API.DatabaseService;
using AppTalk.API.DatabaseService.Interfaces;
using AppTalk.API.Managers;
using AppTalk.Core.Validation;
using AppTalk.Models.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;

namespace AppTalk.API;

public static class ServiceConfigurator
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.ConfigureGeneral();
        builder.ConfigureDatabase();
        builder.ConfigureManagers();
        builder.ConfigureHubs();
        builder.ConfigureLocalServices();
        return builder;
    }

    private static void ConfigureGeneral(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddControllers()
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new InvalidOperationException()))
                };
            });

        builder.Services.AddMvc();
        builder.Services.AddSignalR();
    }

    private static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var databaseConnectionString = configuration.GetConnectionString("PostgreSQL");

        builder.Services.AddDbContext<AppTalkDatabaseContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString);
            options.UseNpgsql(x => x.MigrationsAssembly("AppTalk.API"));
        });

        builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(databaseConnectionString));
        builder.Services.AddScoped<Func<IDbConnection>>(_ => () => new NpgsqlConnection(databaseConnectionString));

        builder.Services.AddScoped<IUserDatabaseService, UserDatabaseService>();
    }

    private static void ConfigureManagers(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserManager>();
    }

    private static void ConfigureHubs(this IHostApplicationBuilder builder)
    {
        // Empty for now
    }

    private static void ConfigureLocalServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<Validator>();
    }
}
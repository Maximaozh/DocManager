using ApplicationDB;
using Data.Repositories;
using DocManager.Data;
using DocManager.Data.Cryptographic;
using DocManager.Data.Jwt;
using DocManager.Endpoints;
using DocManager.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MudBlazor.Services;
using System.Text;

namespace DocManager.Extensions;

public static class ApiExtensions
{
    // Добавляет обработку запросов на сервер
    public static void AddMappedEndpoitns(IEndpointRouteBuilder app)
    {
        app.MapUserEndpoints();
        app.MapDocEndpoints();
    }

    // Добавляет функционал аутентификации и авторизации
    public static void AddAuth(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings").GetValue<string>("Serial")))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Магический куки.
                        // '1251' потому что Я где-то вычитал, что не рекомендуется писать его очевидно
                        context.Token = context.Request.Cookies["1251"];

                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Администратор"));
            options.AddPolicy("UserPolicy", policy => policy.RequireRole("Пользователь"));
        });

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();

        builder.Services.AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>();
    }

    // Добавляет работу с репозиториями БД
    public static void AddDB(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppContextDB>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Myconnection")));

        builder.Services.AddScoped<IUserRepositoriy, UserRepositoriy>();
        builder.Services.AddScoped<IDocumentRepositoriy, DocumentRepositoriy>();
    }


    // Добавляет функционал классов из папки Data
    public static void AddData(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<DocumentService>();
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<IJwtProvider, JwtProvider>();
    }

    // Добавляет неспециализированный функционал, использовать только если не удаётся сгруппировать сервисы в больший метод
    public static void AddUnspecified(WebApplicationBuilder builder)
    {
        // Add MudBlazor services
        builder.Services.AddMudServices();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveWebAssemblyComponents();
        builder.Services.AddHttpClient();
        builder.Services.AddEndpointsApiExplorer();
    }


}

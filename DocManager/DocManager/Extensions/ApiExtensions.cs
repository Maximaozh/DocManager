using DocManager.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.Text;

namespace DocManager.Extensions
{
    public static class ApiExtensions
    {
        public static void AddMappedEndpoitns(this IEndpointRouteBuilder app)
        {
            app.MapUserEndpoints();
        }

        public static void AddApiAuthentication
            (this IServiceCollection services,
            IConfiguration JwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.GetSection(nameof(JwtOptions)).GetValue<string>("Key")))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["1251"];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequiredAdminRole", policy => policy.RequireRole("Администратор"));
            });

            services.AddAuthorization();
        }
    }
}

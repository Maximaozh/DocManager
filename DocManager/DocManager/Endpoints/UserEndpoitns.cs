using DocManager.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Shared.Dto;

namespace DocManager.Endpoints
{
    public static class UserEndpoitns
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/Login", Login);

            app.MapPost("/Registrate", Register).RequireAuthorization(policy => policy.RequireRole("Администратор"));

            return app;
        }

        private static async Task<IResult> Login(UserService userService, UserLogin user, HttpContext httpContext)
        {
            var response = await userService.AuthenticateUser(user, httpContext);
            return response;
        }

        private static async Task<IResult> Register(UserService userService, UserRegistrate user, HttpContext httpContext)
        {
            var response = await userService.Registrate(user, httpContext);
            return response;
        }
    }
}

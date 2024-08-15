using DocManager.Services;
using Shared.Dto;

namespace DocManager.Endpoints
{
    public static class UserEndpoitns
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/login", Login);
            app.MapGet("api/logout", Logout).RequireAuthorization();
            app.MapPost("api/registrate", Register).RequireAuthorization(policy => policy.RequireRole("Администратор"));

            return app;
        }

        private static async Task<IResult> Login(UserService userService, UserLogin user, HttpContext httpContext, IConfiguration configuration)
        {
            IResult response = await userService.AuthenticateUser(user, httpContext, configuration);
            return response;
        }
        private static async Task<IResult> Logout(UserService userService, HttpContext httpContext)
        {
            IResult response = await userService.Logout(httpContext);
            return response;
        }

        private static async Task<IResult> Register(UserService userService, UserRegistrate user)
        {
            IResult response = await userService.Registrate(user);
            return response;
        }

    }
}

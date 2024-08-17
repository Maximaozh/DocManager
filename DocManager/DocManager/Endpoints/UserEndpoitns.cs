using DocManager.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Dto;
using Shared.Dto.User;

namespace DocManager.Endpoints
{
    public static class UserEndpoitns
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/user/login", Login);
            app.MapPost("api/user/loginStartup", LoginStartup);
            app.MapGet("api/user/logout", Logout);
            app.MapPost("api/user/registrate", Registrate).RequireAuthorization(policy => policy.RequireRole("Администратор"));
            app.MapGet("api/users/total", GetCount).RequireAuthorization(policy => policy.RequireRole("Администратор"));
            app.MapPost("api/users/byOffset", GetByOffset).RequireAuthorization(policy => policy.RequireRole("Администратор"));

            return app;
        }

        private static async Task<IResult> Login(HttpContext httpContext, UserService userService, [FromBody] UserLogin user)
        {
            LoginResponse data = await userService.AuthenticateUser(user);
            httpContext.Response.Cookies.Append("1251", data.Token);
            return Results.Json(data);
        }
        private static async Task<IResult> LoginStartup(HttpContext httpContext, [FromBody] BasicWrapper<string> token)
        {
            httpContext.Response.Cookies.Append("1251", token.Value);
            return Results.Ok();
        }

        private static async Task<IResult> Registrate(UserService userService, [FromBody] UserRegistrate user)
        {
            int status = await userService.Registrate(user);
            return status == 0 ? Results.Ok() : Results.BadRequest();
        }
        private static async Task<IResult> GetCount(UserService userService)
        {
            int count = await userService.GetCount();
            return Results.Ok(count);
        }

        private static async Task<IResult> GetByOffset(UserService userService, [FromBody] PaginateFilter pageOffset)
        {
            List<UserInfo> response = await userService.GetByOffset(pageOffset);
            return Results.Json(response);
        }

        private static async Task<IResult> Logout(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("1251");
            return Results.Ok();
        }
    }
}

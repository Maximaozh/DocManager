using DocManager.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Shared.Dto;

namespace DocManager.Endpoints
{
    public static class UserEndpoitns
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/LoginServer", Login);
            app.MapPost("/RegisterServer", Register);

            return app;
        }

        private static async Task<IResult> Login(UserService userService, UserLogin user)
        {
            var result = await userService.AuthenticateUser(user);

            if (result is not null)
                return Results.Json(result);
            else return Results.BadRequest(new { details = "Ошибка при проверке данных. Возможно вы ввели неправильный логин или пароль" });
        }

        private static async Task<IResult> Register(UserService userService, UserRegistrate user)
        {
            var response = await userService.Registrate(user);
            return response;
        }
    }
}

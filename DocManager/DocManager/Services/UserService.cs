using Data.Models;
using Data.Repositories;
using DocManager.Data.Cryptographic;
using DocManager.Data.Jwt;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Dto;

namespace DocManager.Services
{
    public interface IUserService
    {
        public Task<IResult> AuthenticateUser(UserLogin user, HttpContext httpContext, IConfiguration configuration);
        public Task<IResult> Registrate(UserRegistrate user);
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepositoriy _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly AuthenticationStateProvider _serverProvider;
        public UserService(IConfiguration configuration,
            IPasswordHasher passwordHasher,
            IUserRepositoriy userRepositoriy,
            IJwtProvider jwtProvider,
            AuthenticationStateProvider serverAuthenticationStateProvider
            )
        {
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _userRepository = userRepositoriy;
            _jwtProvider = jwtProvider;
            _serverProvider = serverAuthenticationStateProvider;
        }

        public async Task<IResult> AuthenticateUser(UserLogin userLogin, HttpContext httpContext, IConfiguration configuration)
        {
            User? user = await _userRepository.GetByLogin(userLogin.Login);

            if (user == null)
            {
                return Results.BadRequest(new { details = "Не удалось получить пользователя" });
            }

            if (userLogin.Login != user.Login)
            {
                return Results.BadRequest(new { details = "Не удалось найти пользователя с таким логином" });
            }

            if (!_passwordHasher.Verify(userLogin.Password, user.Password))
            {
                return Results.BadRequest(new { details = "Неподходящий пароль" });
            }

            UserInfo data = new UserInfo()
            {
                Login = user.Login,
                Role = user.Role,
                Surname = user.Surname,
                Name = user.Name
            };
            httpContext.Response.Cookies.Append("1251", _jwtProvider.GenerateJWT(data, configuration));
            return Results.Ok();
        }

        public async Task<IResult> Logout(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("1251");
            return Results.Ok();
        }

        public async Task<IResult> Registrate(UserRegistrate user)
        {
            if (user is null)
            {
                return Results.BadRequest(new { details = "Ошибка, пользователь не был получен" });
            }

            User? userFromDB = await _userRepository.GetByLogin(user.Login);

            if (userFromDB is not null)
            {
                return Results.BadRequest(new { details = "Данный логин уже занят" });
            }

            user.Password = _passwordHasher.GenerateHashBCrypt(user.Password);

            await _userRepository.Add(user);
            return Results.Ok();
        }
    }
}

using Data.Repositories;
using DocManager.Client.Data.Login;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocManager.Services
{
    public interface IUserService
    {
        public Task<IResult> AuthenticateUser(UserLogin user, HttpContext httpContext);
        public Task<IResult> Registrate(UserRegistrate user, HttpContext httpContext);
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepositoriy _userRepository;
        private readonly IJwtProvider _jwtProvider;
        public UserService(IConfiguration configuration,
            IPasswordHasher passwordHasher, 
            IUserRepositoriy userRepositoriy,
            IJwtProvider jwtProvider
            )
        {
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _userRepository = userRepositoriy;
            _jwtProvider = jwtProvider;
        }

        public async Task<IResult> AuthenticateUser(UserLogin userLogin, HttpContext httpContext)
        {
            var user = await _userRepository.GetByLogin(userLogin.Login);

            if (user == null)
                return Results.BadRequest(new { details = "Не удалось получить пользователя" });

            if (userLogin.Login != user.Login)
                return Results.BadRequest(new { details = "Не удалось найти пользователя с таким логином" });

            if (!_passwordHasher.Verify(userLogin.Password, user.Password))
                return Results.BadRequest(new { details = "Неподходящий пароль" });

            TokenData data = new TokenData() { Login = user.Login, Password = user.Password, Role = user.Role };
            httpContext.Response.Cookies.Append("1251",_jwtProvider.GenerateJWT(data));
            return Results.Ok();
        }
        public async Task<IResult> Registrate(UserRegistrate user, HttpContext httpContext)
        {
            if(user is null)
                return Results.BadRequest(new { details = "Ошибка, пользователь не был получен" });

            var userFromDB = await _userRepository.GetByLogin(user.Login);
            
            if (userFromDB is not null)
                return Results.BadRequest(new { details = "Данный логин уже занят" });

            user.Password = _passwordHasher.GenerateHashBCrypt(user.Password);

            await _userRepository.Add(user);
            return Results.Ok();
        }
    }
}

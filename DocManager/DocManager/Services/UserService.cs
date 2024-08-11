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
        public Task<TokenResponse> AuthenticateUser(UserLogin user);
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepositoriy _userRepository;
        public UserService(IConfiguration configuration, IPasswordHasher passwordHasher, IUserRepositoriy userRepositoriy)
        {
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _userRepository = userRepositoriy;
        }

        public async Task<TokenResponse> AuthenticateUser(UserLogin userLogin)
        {
            var user = await _userRepository.GetByLogin(userLogin.Login);

            if (user == null)
                return null;

            if (userLogin.Login != user.Login)
                return null;

            if (!_passwordHasher.Verify(userLogin.Password, user.Password))
                return null;

            TokenData data = new TokenData() { Login = user.Login, Password = user.Password, Role = user.Role };
            return new TokenResponse() { Token = GenerateJWT(data) };
        }
        public async Task<IResult> Registrate(UserRegistrate user)
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

        private string GenerateJWT(TokenData data)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, data.Login),
                new Claim (ClaimTypes.SerialNumber, data.Password),
                new Claim (ClaimTypes.Role, data.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}

using DocManager.Client.Data.Login;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using MudBlazor;
using Shared.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DocManager.Services
{
    public interface ILoginService
    {
        public TokenResponse? AuthenticateUser(UserLogin user);
    }
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;

        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResponse? AuthenticateUser(UserLogin user)
        {
            //Логика для проверки данных с базы данных//, здесь запросы на человека с таким логином
            if (false)
                return null;

            // Статика для тестов
            UserLogin expected = new UserLogin() { Login = "admin", Password = "" };
            expected.Login = "Admin2003";

            byte[] salt = BitConverter.GetBytes(42);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: "Admin2004",
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 24,
            numBytesRequested: 256 / 8));

            expected.Password = hashed;

            if (expected.Password != user.Password || user.Login != expected.Login)
                return null;

            TokenData data = new TokenData() { Login = expected.Login, Password = expected.Password, Role = "Admin2005" };
            return new TokenResponse() { Token = GenerateJWT(data) };
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

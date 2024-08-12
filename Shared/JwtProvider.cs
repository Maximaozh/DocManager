using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shared
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;


        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateJWT(TokenData data)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, data.Login),
                new Claim (ClaimTypes.SerialNumber, data.Password),
                new Claim (ClaimTypes.Role, data.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_options.HoursLeft),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}

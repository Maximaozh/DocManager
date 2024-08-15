using Microsoft.IdentityModel.Tokens;
using Shared.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocManager.Data.Jwt;

public class JwtProvider : IJwtProvider
{
    public string GenerateJWT(UserInfo data, IConfiguration configuration)
    {
        List<Claim> claims =
        [
            new Claim("UserId", data.Id),
            new Claim(ClaimTypes.NameIdentifier, data.Login),
            new Claim(ClaimTypes.Role, data.Role),
            new Claim("Name", data.Name),
            new Claim("Surname", data.Surname),

        ];

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings").GetValue<string>("Serial")));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(configuration.GetSection("JwtSettings").GetValue<double>("Expires")),
            signingCredentials: creds
            );

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}

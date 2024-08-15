using Shared.Dto;

namespace DocManager.Data.Jwt;

public interface IJwtProvider
{
    string GenerateJWT(UserInfo data, IConfiguration configuration);
}
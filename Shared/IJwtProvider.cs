using Shared.Dto;

namespace Shared
{
    public interface IJwtProvider
    {
        string GenerateJWT(TokenData data);
    }
}
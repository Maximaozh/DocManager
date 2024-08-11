namespace Shared
{
    public interface IPasswordHasher
    {
        string GenerateHashBCrypt(string password);
        string GenerateHashPbkdf2(string password);
        bool Verify(string password, string passwordHash);
    }
}
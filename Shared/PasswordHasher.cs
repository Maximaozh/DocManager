using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Shared
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GenerateHashBCrypt(string password) => 
            BCrypt.Net.BCrypt.HashPassword(password);

        public string GenerateHashPbkdf2(string password)
        {
            byte[] salt = BitConverter.GetBytes(239082024);
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 512,
                numBytesRequested: 256 / 8));
        }

        public bool Verify(string password, string passwordHash) =>
            BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
    
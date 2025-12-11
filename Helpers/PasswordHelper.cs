using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace SistemaGestionAgricola.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password, byte[]? salt = null)
        {
            if (salt == null)
            {
                salt = GenerateSalt();
            }

            var hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32);

            return Convert.ToBase64String(hashed);
        }

        public static byte[] GenerateSalt()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static bool VerifyPassword(string password, string hashedPassword, byte[] salt)
        {
            var hashToCompare = HashPassword(password, salt);
            return hashedPassword.Equals(hashToCompare);
        }
    }
}
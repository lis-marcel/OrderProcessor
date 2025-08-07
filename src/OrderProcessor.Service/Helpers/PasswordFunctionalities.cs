using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessor.Service.Helpers
{
    // This class contains methods for password hasing.
    // Algorithm used is PBKDF2 with SHA512.

    internal class PasswordFunctionalities
    {
        private const int _keySize = 64;
        private const int _iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

        public static string HashPassword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(_keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: _iterations,
                hashAlgorithm: _hashAlgorithm,
                outputLength: _keySize);

            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: _iterations,
                hashAlgorithm: _hashAlgorithm,
                outputLength: _keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromBase64String(hash));
        }
    }
}

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ExpenseTrackingApp.Services.Helpers
{
    public static class SecurityHelper
    {
        public static string GenerateSalt(int nSalt)
        {
            var saltBytes = new byte[nSalt];


            using (var provider = RandomNumberGenerator.Create())
            {
                provider.GetNonZeroBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                password: password!,
                                                salt: saltBytes,
                                                prf: KeyDerivationPrf.HMACSHA256,
                                                iterationCount: 100000,
                                                numBytesRequested: 256 / 8));
            return hashed;
        }

        public static bool VerifyHashedPassword(string requestPassword, string passwordHash, string passwordSalt)
        {
            var requestPasswordHash = HashPassword(requestPassword, passwordSalt);
            return passwordHash.Equals(requestPasswordHash);
        }
    }
}



namespace LocalMarket.Infrastructure.Utils
{
    public static class PasswordHasher
    {
        public static string EncriptPassword(string password)
        {
             return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }
    }
}

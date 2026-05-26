

namespace LocalMarket.Infrastructure.Utils
{
    public static class Encript
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

using System.Security.Cryptography;
using System.Text;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.Services
{
    public class PasswordHasherSHA256 : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return string.Empty;

            byte[] hash = Array.Empty<Byte>();

            using(var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                hash = sha.ComputeHash(bytes);
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public bool Verify(string password, string hash)
            => string.Equals(HashPassword(password), hash);
    }
}

using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Infrastructure.Users.Service
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}

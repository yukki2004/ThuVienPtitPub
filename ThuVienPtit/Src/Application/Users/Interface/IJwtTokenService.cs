using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Users.Interface
{
    public interface IJwtTokenService
    {
        string GenerateToken(users user);
        string GenerateRefreshToken();
    }
}

using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Users.DTOs.DtoRespone
{
    public class LoginUserRespone
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public UserInfoDto userInfoDto { get; set; } = null!;
    }
}

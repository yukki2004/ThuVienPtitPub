using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Users.DTOs.DtoRespone
{
    public class GoogleLoginResponeDto
    {
        public string access_token { get; set; } = null!;
        public string refresh_token { get; set; } = null!;
        public UserInfoDto UserInfoDto { get; set; } = null!;
    }
}

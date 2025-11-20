using ThuVienPtit.Src.Application.Users.DTOs.Entities;
using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Users.DTOs.DtoRespone
{
    public class UserInfoDto
    {
        public Guid user_id { get; set; }
        public string name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string username { get; set; } = null!;
        public Role role { get; set; }
        public string? major { get; set; }
        public string? img { get; set; }
        public login_type login_type { get; set; }
        public string? google_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
      
    }
}

using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Users.DTOs.DtoRespone
{
    public class UpdateUserResponeDto
    {
        public Guid userid { get; set; }
        public string name { get; set; } = null!;
        public string username { get; set; } = null!;
        public string email { get; set; } = null!;
        public string? imgurl { get; set; }
        public string? major {  get; set; }
        public DateTime  create_at { get; set; }
        public login_type login_Type { get; set; }
        public string? google_id { get; set; }
        public Role role { get; set; }
        public DateTime update_at { get; set; }
    }
}

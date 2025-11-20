using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Domain.Entities
{
    public class users
    {
        public Guid user_id { get; set; }
        public string name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string username { get; set; } = null!;
        public string? password_hash { get; set; }
        public Role role { get; set; }
        public string? major { get; set; }
        public string? img { get; set; }
        public login_type login_type { get; set; }
        public string? google_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        // Navigation
        public ICollection<refresh_tokens>? refresh_tokens { get; set; } = new List<refresh_tokens>();
        public ICollection<password_reset_tokens>? password_reset_tokens { get; set; } = new List<password_reset_tokens>();
        public ICollection<documents> documents { get; set; } = new List<documents>();
    }
}

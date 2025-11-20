using System.ComponentModel.DataAnnotations.Schema;

namespace ThuVienPtit.Src.Domain.Entities
{
    public class refresh_tokens
    {
        public Guid token_id { get; set; }
        public Guid user_id { get; set; }
        public string refresh_token { get; set; } = null!;
        public DateTime expires_at { get; set; }
        public bool revoked { get; set; }
        public DateTime created_at { get; set; }

        // Navigation
        public users user { get; set; } = null!;
    }
}

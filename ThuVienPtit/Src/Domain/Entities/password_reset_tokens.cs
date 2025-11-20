namespace ThuVienPtit.Src.Domain.Entities
{
    public class password_reset_tokens
    {
        public Guid token_id { get; set; }
        public Guid user_id { get; set; }
        public string reset_code { get; set; } = null!;
        public DateTime expires_at { get; set; }
        public bool used { get; set; }
        public DateTime created_at { get; set; }

        // Navigation
        public users user { get; set; } = null!;
    }
}

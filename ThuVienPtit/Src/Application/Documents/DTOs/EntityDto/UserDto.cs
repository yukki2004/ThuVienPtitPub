namespace ThuVienPtit.Src.Application.Documents.DTOs.EntityDto
{
    public class UserDto
    {
        public Guid user_id { get; set; }
        public string userName { get; set; } = null!;
        public string name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string? avt { get; set; }
    }
}

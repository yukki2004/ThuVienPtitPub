using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Documents.DTOs.Respone.GetDocumets
{
    public class UserDto
    {
        public Guid user_id { get; set; }
        public string name { get; set; } = null!;
        public string? avt { get; set; }
    }
}

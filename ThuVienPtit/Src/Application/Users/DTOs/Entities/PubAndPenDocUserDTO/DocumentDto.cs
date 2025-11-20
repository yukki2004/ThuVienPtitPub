using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Users.DTOs.Entities.PubAndPenDocUserDTO
{
    public class DocumentDto
    {
        public Guid document_id { get; set; }
        public string title { get; set; } = null!;
        public string? description { get; set; }
        public string slug { get; set; } = null!;
        public string avt_document { get; set; } = null!;
        public DateTime created_at { get; set; }
        public Users.DTOs.Entities.PubAndPenDocUserDTO.CoursesDto course { get; set; } = null!;
        public List<Users.DTOs.Entities.PubAndPenDocUserDTO.TagDto> tags { get; set; } = new List<TagDto>();
    }
}

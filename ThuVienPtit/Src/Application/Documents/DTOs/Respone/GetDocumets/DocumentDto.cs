using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Documents.DTOs.Respone.GetDocumets
{
    public class DocumentDto
    {
        public Guid document_id { get; set; }
        public string title { get; set; } = null!;
        public string? description { get; set; }
        public string slug { get; set; } = null!;
        public string avt_document { get; set; } = null!;
        public DateTime created_at { get; set; }
        public List<Application.Documents.DTOs.Respone.GetDocumets.TagDto> tags { get; set; } = new List<TagDto>();
        public Application.Documents.DTOs.Respone.GetDocumets.UserDto? user { get; set; }
        public Application.Documents.DTOs.Respone.GetDocumets.CourseDto course { get; set; } = null!;
    }
}

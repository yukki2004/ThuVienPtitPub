using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Documents.DTOs.Respone.GetDocumentById
{
    public class DocumentDto
    {
        public Guid document_id { get; set; }
        public string title { get; set; } = null!;
        public string? description { get; set; }
        public string file_path { get; set; } = null!;
        public string avt_document { get; set; } = null!;
        public string slug { get; set; } = null!;
        public status_document status { get; set; }
        public Guid? user_id { get; set; }
        public DateTime created_at { get; set; }
        public Documents.DTOs.Respone.GetDocumentById.CourseDto course { get; set; } = null!;
        public List<Documents.DTOs.Respone.GetDocumentById.TagDto> tags { get; set; } = new List<Documents.DTOs.Respone.GetDocumentById.TagDto>();
    }
}

using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Documents.DTOs.Respone
{
    public class GetRelatedDocumentDto
    {
        public Guid document_id { get; set; }
        public string title { get; set; } = null!;
        public string? description { get; set; }
        public string category { get; set; } = null!;
        public string slug { get; set; } = null!;
        public string avt_document { get; set; } = null!;
        public status_document status { get; set; }
        public DateTime created_at { get; set; }
    }
}

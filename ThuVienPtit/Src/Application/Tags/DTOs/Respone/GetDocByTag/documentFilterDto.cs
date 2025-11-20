using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag
{
    public class documentFilterDto
    {
        public Guid document_id { get; set; }
        public string title { get; set; } = null!;
        public string? description { get; set; }
        public string slug { get; set; } = null!;
        public string avt_document { get; set; } = null!;
        public DateTime created_at { get; set; }
        public status_document status { get; set; }
    }
}

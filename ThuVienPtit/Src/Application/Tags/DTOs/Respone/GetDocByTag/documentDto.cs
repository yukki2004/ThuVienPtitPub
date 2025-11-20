namespace ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag
{
    public class documentDto
    {
        public Guid document_id { get; set; }
        public string title { get; set; } = null!;
        public string? description { get; set; }
        public string slug { get; set; } = null!;
        public string avt_document { get; set; } = null!;
        public DateTime created_at { get; set; }
        public List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto> tags { get; set; } = new List<tagDto>();
        public ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.courseDto course { get; set; } = null!;

    }
}

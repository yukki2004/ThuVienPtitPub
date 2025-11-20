namespace ThuVienPtit.Src.Application.Documents.DTOs.EntityDto
{
    public class TagDocDto
    {
        public int tag_id { get; set; }
        public string name { get; set; } = null!;
        public string slug { get; set; } = null!;
        public DateTime created_at { get; set; }
        public string? description { get; set; } = null!;
    }
}

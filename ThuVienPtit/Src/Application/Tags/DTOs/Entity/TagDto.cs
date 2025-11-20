namespace ThuVienPtit.Src.Application.Tags.DTOs.Entity
{
    public class TagDto
    {
        public int tag_id { get; set; }
        public string name { get; set; } = null!;
        public string slug { get; set; } = null!;
        public DateTime created_at { get; set; }
        public string? description { get; set; } = null!;
    }
}

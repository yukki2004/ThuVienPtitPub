namespace ThuVienPtit.Src.Domain.Entities
{
    public class tags
    {
        public int tag_id { get; set; }
        public string name { get; set; } = null!;
        public string slug { get; set; } = null!;
        public DateTime created_at { get; set; }
        public string? description { get; set; } = null!;
        // Navigation properties
        public ICollection<document_tags> document_Tags { get; set; } = new List<document_tags>();
    }
}

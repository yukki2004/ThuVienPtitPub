namespace ThuVienPtit.Src.Domain.Entities
{
    public class course_categories
    {
        public int category_id { get; set; }
        public string name { get; set; } = null!;
        public string? description { get; set; }
        public DateTime created_at { get; set; }
        // navigation
        public ICollection<courses>? courses { get; set; } = new List<courses>();
    }
}

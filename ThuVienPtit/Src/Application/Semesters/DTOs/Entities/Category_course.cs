namespace ThuVienPtit.Src.Application.Semesters.DTOs.Entities
{
    public class Category_course
    {
        public int category_id { get; set; }
        public string name { get; set; } = null!;
        public string? description { get; set; }
        public DateTime created_at { get; set; }
    }
}

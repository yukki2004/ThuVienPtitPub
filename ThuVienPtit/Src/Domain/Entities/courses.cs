namespace ThuVienPtit.Src.Domain.Entities
{
    public class courses
    {
        public Guid course_id { get; set; }
        public string name { get; set; } = null!;
        public string? description { get; set; }
        public int credits { get; set; }
        public string slug { get; set; } = null!;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Guid semester_id { get; set; }
        public int category_id { get; set; }

        // navigation
        public semesters semester { get; set; } = null!;
        public course_categories category { get; set; } = null!;
        public ICollection<documents> documents { get; set; } = new List<documents>();
    }
}

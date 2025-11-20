namespace ThuVienPtit.Src.Application.Semesters.DTOs.Entities
{
    public class CourseDto
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
        public Category_course category_Course { get; set; } = null!;
    }
}

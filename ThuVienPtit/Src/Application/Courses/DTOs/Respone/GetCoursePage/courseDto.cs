namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage
{
    public class courseDto
    {
        public Guid course_id { get; set; }
        public string name { get; set; } = null!;
        public string? description { get; set; }
        public int credits { get; set; }
        public string slug { get; set; } = null!;
        public DateTime created_at { get; set; }
        public ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage.category_course category_Course { get; set; } = null!;
        public ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage.semesterDto semesterDto { get; set; } = null!;

    }
}

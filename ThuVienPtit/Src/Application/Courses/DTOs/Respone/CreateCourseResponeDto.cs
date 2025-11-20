using ThuVienPtit.Src.Application.Courses.DTOs.Entity;

namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone
{
    public class CreateCourseResponeDto
    {
        public Guid course_id { get; set; }
        public string name { get; set; } = null!;
        public string? description { get; set; }
        public int credits { get; set; }
        public string slug { get; set; } = null!;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Guid semester_id { get; set; }
        public CategoryDto CategoryCourse { get; set; } = null!;
    }
}

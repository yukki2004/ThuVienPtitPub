using ThuVienPtit.Src.Application.Courses.DTOs.Entity;

namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse
{
    public class CourseDetailDto
    {
        public Guid CourseId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Credits { get; set; }
        public string Slug { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public CategoryDto Category { get; set; } = null!;
        public SemesterDto Semester { get; set; } = null!;
    }
}

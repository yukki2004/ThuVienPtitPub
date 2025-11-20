using ThuVienPtit.Src.Application.Courses.DTOs.Entity;

namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone
{
    public class GetSemesterByCourseDto
    {
        public Courses.DTOs.Entity.SemesterDto Semester { get; set; } = null!;
        public Courses.DTOs.Entity.CourseDto Course { get; set; } = null!;
    }
}

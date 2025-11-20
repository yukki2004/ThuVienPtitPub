namespace ThuVienPtit.Src.Application.Semesters.DTOs.Respone.GetFullCouresSemester
{
    public class CourseDto
    {
        public Guid course_id { get; set; }
        public string name { get; set; } = null!;
        public int credits { get; set; }
        public string slug { get; set; } = null!;
        public CategoryCoursesDto category_Course { get; set; } = null!;
    }
}

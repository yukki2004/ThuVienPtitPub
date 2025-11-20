namespace ThuVienPtit.Src.Application.Courses.DTOs.Request
{
    public class UpdateCourseRequestDto
    {
        public string? description { get; set; }
        public int credits { get; set; }
        public int category_id { get; set; }
    }
}

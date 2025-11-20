namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone
{
    public class GetAllCourseRespone
    {
        public Guid course_id { get; set; }
        public string name { get; set; } = null!;
        public string slug { get; set; } = null!;

    }
}

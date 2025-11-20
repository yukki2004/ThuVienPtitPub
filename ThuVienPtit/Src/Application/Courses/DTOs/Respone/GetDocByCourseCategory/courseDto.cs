namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetDocByCourseCategory
{
    public class courseDto
    {
        public Guid course_id { get; set; }
        public string name { get; set; } = null!;
        public string? description { get; set; }
        public int credits { get; set; }
        public string slug { get; set; } = null!;
        public List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetDocByCourseCategory.documentDto> documents { get; set; } = new List<documentDto>();
    }
}

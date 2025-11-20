using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetDocByCourseCategory
{
    public class documentDto
    {
        public Guid document_id { get; set; }
        public string title { get; set; } = null!;
        public string? description { get; set; }
        public string avt_document { get; set; } = null!;
        public string slug { get; set; } = null!;
        public DateTime created_at { get; set; }
        public List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetDocByCourseCategory.TagDocDto> tags { get; set; } = new List<TagDocDto>();


    }
}

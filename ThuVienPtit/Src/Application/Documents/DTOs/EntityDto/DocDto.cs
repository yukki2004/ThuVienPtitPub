using ThuVienPtit.Src.Application.Semesters.DTOs.Entities;
using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Documents.DTOs.EntityDto
{
    public class DocDto
    {
        public Guid document_id { get; set; }
        public string title { get; set; } = null!;
        public string? description { get; set; }
        public string file_path { get; set; } = null!;
        public string category { get; set; } = null!;
        public status_document status { get; set; }
        public Guid course_id { get; set; }
        public Guid? user_id { get; set; }
        public string slug { get; set; } = null!;
        public string avt_document { get; set; } = null!;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string relativePathSlug { get; set; } = null!;
        public string relativePathId { get; set; } = null!;
        public CourseDto course { get; set; } = null!;
        public UserDto? user { get; set; }
        public List<TagDocDto> tags { get; set; } = new List<TagDocDto>();
    }
}

using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Users.DTOs.Entities
{
    public class DocumentUserDto
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
        public List<TagDocumentUserDto> tags { get; set; } = new List<TagDocumentUserDto>();
        public CourseUserDto course { get; set; } = null!;
        public UserInfoDto UserInfoDto { get; set; } = null!;
    }
}

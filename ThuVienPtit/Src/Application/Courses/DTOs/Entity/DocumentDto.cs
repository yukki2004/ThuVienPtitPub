using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Courses.DTOs.Entity
{
    public class DocumentDto
    {
        public Guid DocumentId { get; set; }
        public string Title { get; set; } = null!;
        public string Category { get; set; } = null!;
        public status_document Status { get; set; }
        public string Slug { get; set; } = null!;
        public string AvtUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}

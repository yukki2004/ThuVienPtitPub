using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone
{
    public class GetDocumentsByCourseResultDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalItem { get; set; }
        public List<ThuVienPtit.Src.Application.Courses.DTOs.Entity.DocumentDto> Items { get; set; } = new List<ThuVienPtit.Src.Application.Courses.DTOs.Entity.DocumentDto>();
    }
}

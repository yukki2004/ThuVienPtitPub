using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;
using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetAllDocumentByCourseQueries : IRequest<GetDocumentsByCourseResultDto>
    {
        public Guid course_id { get; set; }
        public int pageNumber { get; set; }
        public string? category { get; set; }
        public int? status { get; set; }
    }
}

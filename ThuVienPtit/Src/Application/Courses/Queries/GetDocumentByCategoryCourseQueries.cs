using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetDocumentByCategoryCourseQueries : IRequest<GetDocumentRespone>
    {
        public string slug { get; set; } = null!;
        public string category { get; set; } = null!;
        public int pageNumber { get; set; }
    }
}

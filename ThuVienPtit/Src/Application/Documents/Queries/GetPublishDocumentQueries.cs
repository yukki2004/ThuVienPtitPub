using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Documents.Queries
{
    public class GetPublishDocumentQueries : IRequest<Documents.DTOs.Respone.GetDocumentRespone>
    {
        public int pageNumber { get; set; }
    }
}

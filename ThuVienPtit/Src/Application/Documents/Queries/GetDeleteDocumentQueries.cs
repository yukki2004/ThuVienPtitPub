using MediatR;
using ThuVienPtit.Src.Application.Documents.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Documents.Queries
{
    public class GetDeleteDocumentQueries : IRequest<GetDocumentRespone>
    {
        public int pageNumber { get; set; }
    }
}

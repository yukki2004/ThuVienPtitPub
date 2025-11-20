using MediatR;
using ThuVienPtit.Src.Application.Tags.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Tags.Queries
{
    public class GetDocumentByTagQueries : IRequest<GetDocumentByTag>
    {
        public string slug { get; set; } = null!;
        public int pageNumber { get; set; }
    }
}

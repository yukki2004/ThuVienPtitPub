using MediatR;
using ThuVienPtit.Src.Application.Documents.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Documents.Queries
{
    public class GetRelatedDocumentQueries : IRequest<List<GetRelatedDocumentDto>>
    {
        public Guid document_id { get; set; }
    }
}

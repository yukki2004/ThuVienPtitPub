using MediatR;
using ThuVienPtit.Src.Application.Documents.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Documents.Queries
{
    public class GetDocumentByIdQueries : IRequest<GetDocumentByIdQueriesDto>
    {
        public string slug { get; set; } = null!;
        public string userRole { get; set; } = null!;
        public Guid? user_id { get; set; }
    }
}

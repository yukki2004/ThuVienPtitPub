using MediatR;
using System.Reflection.Metadata;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Application.Users.DTOs.Entities;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class GetDocumentPublishUserQueries : IRequest<GetDocumentUser>
    {
        public Guid user_id { get; set; }
        public int pageNumber { get; set; }
    }
}

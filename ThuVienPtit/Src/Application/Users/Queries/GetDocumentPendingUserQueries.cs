using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class GetDocumentPendingUserQueries :  IRequest<GetDocumentUser>
    {
        public Guid user_id { get; set; }
        public int pageNumber { get; set; }
    }
}

using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class GetListUserQueries : IRequest<ListUserDto>
    {
        public int pageNumber { get; set; }
    }
}

using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class GetUserInfoQueries : IRequest<UserInfoDto>
    {
        public Guid user_id { get; set; }
    }
}

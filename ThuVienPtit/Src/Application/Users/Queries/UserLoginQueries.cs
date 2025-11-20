using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class UserLoginQueries : IRequest<LoginUserRespone>
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class RefreshTokenCommand : IRequest<RefreshTokenDto>
    {
        public string refresh_token { get; set; } = null!;
    }
}

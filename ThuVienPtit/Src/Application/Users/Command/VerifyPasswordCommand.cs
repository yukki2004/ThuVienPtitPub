using MediatR;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class VerifyPasswordCommand : IRequest<bool>
    {
        public string email { get; set; } = null!;
        public string emailCode { get; set; } = null!;
    }
}

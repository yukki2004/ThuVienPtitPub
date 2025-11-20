using MediatR;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class ForgotPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; } = null!;
    }
}

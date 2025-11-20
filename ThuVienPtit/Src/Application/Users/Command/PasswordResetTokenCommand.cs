using MediatR;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class PasswordResetTokenCommand : IRequest<bool>
    {
        public string email { get; set; } = null!;
        public string emailCode { get; set; } = null!;
        public string newPassword { get; set; } = null!;
        public string confirmNewPassword { get; set; } = null!;
    }
}

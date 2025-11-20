using MediatR;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class ChangePasswordCommand : IRequest<string>
    {
        public string oldPassword { get; set; } = null!;
        public string newPassword { get; set; } = null!;
        public string confirmPassword { get; set; } = null!;
        public Guid userid { get; set; }
    }
}
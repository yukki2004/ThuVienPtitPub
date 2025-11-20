using MediatR;
using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class RegisterUserCommand : IRequest<Unit>
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;   
        public Role role { get; set; } = Role.student;
    }
}

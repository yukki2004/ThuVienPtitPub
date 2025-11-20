using MediatR;
using ThuVienPtit.Src.Application.Users.Interface;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class RegisterUserCommandHandle : IRequestHandler<RegisterUserCommand, Unit>
    {
        private readonly IUserRespository userRespository;
        public RegisterUserCommandHandle(IUserRespository userRespository)
        {
            this.userRespository = userRespository;
        }
        public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Domain.Entities.users
            {
                user_id = Guid.NewGuid(),
                username = request.Username,
                email = request.Email,
                password_hash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                name = request.Name,
                role = Domain.Enum.Role.student,
                login_type = Domain.Enum.login_type.local,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };
            await userRespository.RegisterUser(user);
            return Unit.Value;
        }
    }
}

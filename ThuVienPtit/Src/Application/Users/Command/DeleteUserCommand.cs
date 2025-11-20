using MediatR;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid user_id { get; set; }
    }
}

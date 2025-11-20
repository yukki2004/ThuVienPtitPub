using MediatR;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class LogoutUserQueries : IRequest<bool>
    {
        public string refresh_token { get; set; } = null!;
    }
}

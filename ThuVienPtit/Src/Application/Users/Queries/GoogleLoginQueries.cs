using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;

namespace ThuVienPtit.Src.Application.Users.Queries
{
    public class GoogleLoginQueries : IRequest<GoogleLoginResponeDto>
    {
        public string id_token { get; set; } = null!;
    }
}

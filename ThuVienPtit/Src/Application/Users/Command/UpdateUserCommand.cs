using MediatR;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;

namespace ThuVienPtit.Src.Application.Users.Command
{
    public class UpdateUserCommand : IRequest<UpdateUserResponeDto>
    {
        public string userName { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? major { get; set; }
        public string? imageName { get; set; }
        public Stream? stream { get; set; }

        public Guid userid { get; set; }
    }
}

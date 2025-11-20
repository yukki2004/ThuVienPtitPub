using MediatR;
using ThuVienPtit.Src.Application.Tags.DTOs.Entity;

namespace ThuVienPtit.Src.Application.Tags.Command
{
    public class CreateTagCommand : IRequest<TagDto>
    {
        public string name { get; set; } = null!;
        public string? description { get; set; }
    }
}

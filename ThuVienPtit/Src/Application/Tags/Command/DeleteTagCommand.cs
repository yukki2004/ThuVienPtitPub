using MediatR;

namespace ThuVienPtit.Src.Application.Tags.Command
{
    public class DeleteTagCommand : IRequest<bool>
    {
        public int tag_id { get; set; }
    }
}

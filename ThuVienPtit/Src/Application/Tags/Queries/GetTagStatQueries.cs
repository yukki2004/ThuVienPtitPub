using MediatR;

namespace ThuVienPtit.Src.Application.Tags.Queries
{
    public class GetTagStatQueries : IRequest<ThuVienPtit.Src.Application.Tags.DTOs.Respone.TagStatDto>
    {
        public int tagId { get; set; }
    }
}

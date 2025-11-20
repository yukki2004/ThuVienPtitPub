using MediatR;

namespace ThuVienPtit.Src.Application.Tags.Queries
{
    public class GetAllListTagQueries : IRequest<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetListTagAllDto>
    {
        public int pageNumber { get; set; }
    }
}

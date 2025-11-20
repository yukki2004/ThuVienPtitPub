using MediatR;

namespace ThuVienPtit.Src.Application.Tags.Queries
{
    public class GetAllTagQueries : IRequest<List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto>>
    {
    }
}

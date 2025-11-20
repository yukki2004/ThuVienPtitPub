using MediatR;
using ThuVienPtit.Src.Application.Tags.Interface;

namespace ThuVienPtit.Src.Application.Tags.Queries
{
    public class GetAllListTagQueriesHandle : IRequestHandler<GetAllListTagQueries, ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetListTagAllDto>
    {
        private readonly ITagRespository tagRespository;
        public GetAllListTagQueriesHandle(ITagRespository tagRespository)
        {
            this.tagRespository = tagRespository;
        }
        public async Task<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetListTagAllDto> Handle(GetAllListTagQueries queries, CancellationToken cancellationToken)
        {
            var data = await tagRespository.GetAllListTagAdminAsync(queries.pageNumber);
            var respone = new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetListTagAllDto
            {
                pageNumber = queries.pageNumber,
                pageSize = data.pageSize,
                totalPage = data.totalPage,
                totalItem = data.totalItem,
                tags = data.Item5.Select(dt => new ThuVienPtit.Src.Application.Tags.DTOs.Entity.TagDto
                {
                    tag_id = dt.tag_id,
                    name = dt.name,
                    description = dt.description,
                    slug = dt.slug,
                    created_at = dt.created_at,
                }).ToList()
            };
            return respone;
        }
    }
}

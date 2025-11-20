using MediatR;
using ThuVienPtit.Src.Application.Tags.Interface;

namespace ThuVienPtit.Src.Application.Tags.Queries
{
    public class GetTagStatQueriesHandle : IRequestHandler<GetTagStatQueries, ThuVienPtit.Src.Application.Tags.DTOs.Respone.TagStatDto>
    {
        private readonly ITagRespository tagRespository;

        public GetTagStatQueriesHandle(ITagRespository tagRespository)
        {
            this.tagRespository = tagRespository;
        }

        public async Task<ThuVienPtit.Src.Application.Tags.DTOs.Respone.TagStatDto> Handle(GetTagStatQueries queries, CancellationToken cancellationToken)
        {
            var tagData = await tagRespository.GetTagStatAsync(queries.tagId);
            if (tagData == null)
            {
                throw new KeyNotFoundException($"Tag with ID {queries.tagId} not found.");
            }

            return tagData;
        }
    }
}

using MediatR;
using System.Collections.Generic;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Tags.Queries
{
    public class GetAllTagQueriesHandle : IRequestHandler<GetAllTagQueries, List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto>>
    {
        private readonly Interface.ITagRespository tagRespository;
        private readonly ICacheService cacheService;
        public GetAllTagQueriesHandle(Interface.ITagRespository tagRespository, ICacheService cacheService)
        {
            this.tagRespository = tagRespository;
            this.cacheService = cacheService;
        }
        public async Task<List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto>> Handle(GetAllTagQueries request, CancellationToken cancellationToken)
        {
            const string cacheKey = "thuvienptit:tag:all";
            var cacheData = await cacheService.GetAsync<List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto>>(cacheKey);
            if(cacheData != null)
            {
                Console.WriteLine("trúng cache");
                return cacheData;
            }
            var tags = await tagRespository.GetAllTagsAsync(cancellationToken);
            var tagDtos = tags.Select(tag => new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto
            {
                tag_id = tag.tag_id,
                name = tag.name,
                slug = tag.slug,
            }).ToList();
            await cacheService.SetAsync(cacheKey, tagDtos, TimeSpan.FromHours(15));
            return tagDtos;
        }
    }
}

using MediatR;
using System.Net.WebSockets;
using ThuVienPtit.Src.Application.Interface;
using ThuVienPtit.Src.Application.Tags.DTOs.Entity;
using ThuVienPtit.Src.Application.Tags.Interface;

namespace ThuVienPtit.Src.Application.Tags.Command
{
    public class CreateTagCommandHandle : IRequestHandler<CreateTagCommand, TagDto>
    {
        private readonly ITagRespository tagRespository;
        private readonly ISlugHelper slugHelper;
        private readonly ICacheService cacheService;
        public CreateTagCommandHandle(ITagRespository tagRespository, ISlugHelper slugHelper, ICacheService cacheService)
        {
            this.tagRespository = tagRespository;
            this.slugHelper = slugHelper;
            this.cacheService = cacheService;
        }
        public async Task<TagDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = new Domain.Entities.tags
            {
                name = request.name,
                description = request.description,
                created_at = DateTime.UtcNow,
                slug = slugHelper.GenerateSlug(request.name)
            };
            await tagRespository.AddTagAsync(tag);
            // xóa cache 
            var cacheKey = "thuvienptit:tag:all";
            await cacheService.RemoveAsync(cacheKey);
            return new TagDto
            {
                tag_id = tag.tag_id,
                name = tag.name,
                description = tag.description,
                created_at = tag.created_at,
                slug = tag.slug
            };
        }

    }
}

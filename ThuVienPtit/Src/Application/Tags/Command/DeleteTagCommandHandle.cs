using MediatR;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Tags.Command
{
    public class DeleteTagCommandHandle : IRequestHandler<DeleteTagCommand, bool>
    {
        private readonly Tags.Interface.ITagRespository tagRespository;
        private readonly ICacheService cacheService;
        public DeleteTagCommandHandle(Tags.Interface.ITagRespository tagRespository, ICacheService cacheService)
        {
            this.tagRespository = tagRespository;
            this.cacheService = cacheService;
        }
        public async Task<bool> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await tagRespository.GetTagByIdAsync(request.tag_id);
            if(tag == null)
            {
                return false;
            }
            await tagRespository.DeleteTagAsync(request.tag_id);
            var cacheKey = "thuvienptit:tag:all";
            await cacheService.RemoveAsync(cacheKey);
            return true;

        }
    }
}

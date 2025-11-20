using MediatR;
using ThuVienPtit.Src.Application.Documents.Interface;
using ThuVienPtit.Src.Application.Helpers;
using ThuVienPtit.Src.Application.Interface;
using ThuVienPtit.Src.Application.Helpers;

namespace ThuVienPtit.Src.Application.Documents.Command
{
    public class ApproveDocumentCommandHandle : IRequestHandler<ApproveDocumentCommand, bool>
    {
        private readonly IDocumentRespository documentRespository;
        private const string PublishVersionKey = "PublishDoc:version";
        private const string PendingVersionKey = "PendingDoc:version";
        private readonly ICacheService cacheService;
        private readonly Application.Courses.Interface.ICourseRespository courseRespository;
        public ApproveDocumentCommandHandle(IDocumentRespository documentRespository, ICacheService cacheService, Courses.Interface.ICourseRespository courseRespository)
        {
            this.documentRespository = documentRespository;
            this.cacheService = cacheService;
            this.courseRespository = courseRespository;
        }
        public async Task<bool> Handle(ApproveDocumentCommand command, CancellationToken cancellationToken)
        {
            var document = await documentRespository.GetDocumentByIdAsync(command.document_id);
            if(document == null)
            {
                return false;
            }
            if(document.status != Domain.Enum.status_document.pending)
            {
                return false;
            }
            var newDocument = await courseRespository.GetCoursesById(document.course_id);
            if (newDocument == null)
            {
                return false;
            }
            document.status = Domain.Enum.status_document.approved;
            await documentRespository.UpdateDocumentAsync(document, cancellationToken);

            // xử lý cache khi duyệt bài viết
            // duyệt bài viết thì tăng version cache cho các bài viết publish
            var versionPub = await cacheService.GetVersionAsync(PublishVersionKey) ?? 1;
            await cacheService.SetVersionAsync(PublishVersionKey, versionPub + 1);
            // duyệt bài viết thì tăng version cache cho các bài viết pending
            var versionPen = await cacheService.GetVersionAsync(PendingVersionKey) ?? 1;
            await cacheService.SetVersionAsync(PendingVersionKey, versionPen + 1);
            // duyệt bài viết tăng version cache cho filter document theo category của course
            if (!CacheVersionHelper.Map.TryGetValue(document.category, out var categorySlug))
                categorySlug = document.category;
            var categoryVersionKey = $"course:{newDocument.slug}:category:{categorySlug}:version";
            var categoryVersion = await cacheService.GetVersionAsync(categoryVersionKey) ?? 1;
            await cacheService.SetVersionAsync(categoryVersionKey, categoryVersion + 1);
            return true;
        }
    }
}

using MediatR;
using ThuVienPtit.Src.Application.Documents.Interface;
using ThuVienPtit.Src.Application.Helpers;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Documents.Command
{
    public class DeleteDocumentCommandHandle : IRequestHandler<DeleteDocumentCommand, bool>
    {
        private readonly IDocumentRespository documentRespository;
        private const string DeleteVersionKey = "DeleteDoc:version";
        private const string PublishVersionKey = "PublishDoc:version";
        private readonly ICacheService cacheService;
        private readonly Application.Courses.Interface.ICourseRespository courseRespository;

        public DeleteDocumentCommandHandle(IDocumentRespository documentRespository, ICacheService cacheService, Application.Courses.Interface.ICourseRespository courseRespository)
        {
            this.documentRespository = documentRespository;
            this.cacheService = cacheService;
            this.courseRespository = courseRespository;
        }
        public async Task<bool> Handle(DeleteDocumentCommand command, CancellationToken cancellationToken)
        {
            var document = await documentRespository.GetDocumentByIdAsync(command.document_id);
            if(document == null)
            {
                throw new Exception("Tài liệu không tồn tại");
            }
            var newDocument = await courseRespository.GetCoursesById(document.course_id);
            if(newDocument == null)
            {
                throw new Exception("tài liệu không thuộc về khóa học nào");
            }
            if (command.user_role != "admin")
            {
                if (document.user_id != command.user_id)
                {
                    return false;
                }
                document.status = Domain.Enum.status_document.deleted;
                await documentRespository.UpdateDocumentAsync(document, cancellationToken);
            }
            else
            {
                document.status = Domain.Enum.status_document.deleted;
                await documentRespository.UpdateDocumentAsync(document, cancellationToken);
            }
            // xử lý xóa cache
            // tăng version cache cho các bài viết đã delete
            var versionDel = await cacheService.GetVersionAsync(DeleteVersionKey) ?? 1;
            await cacheService.SetVersionAsync(DeleteVersionKey, versionDel + 1);
            // tăng version cache cho các bài viết public
            var versionPub = await cacheService.GetVersionAsync(PublishVersionKey) ?? 1;
            await cacheService.SetVersionAsync(PublishVersionKey, versionPub + 1);
            // xóa cache của tài liệu đã được cache khi bài viết bị delete
            var cacheKey = $"ViewDoc:Slug:{document.slug}";
            await cacheService.RemoveAsync(cacheKey);
            // tăng version của cache lọc document theo category của môn học
            if (!CacheVersionHelper.Map.TryGetValue(document.category, out var categorySlug))
                categorySlug = document.category;
            var categoryVersionKey = $"course:{newDocument.slug}:category:{categorySlug}:version";
            var categoryVersion = await cacheService.GetVersionAsync(categoryVersionKey) ?? 1;
            await cacheService.SetVersionAsync(categoryVersionKey, categoryVersion + 1);
            // xóa cache list các bài viết liên quan khi document bị xóa
            var cacheRelativeKey = $"RelatedDocuments:{document.document_id}";
            await cacheService.RemoveAsync(cacheRelativeKey);
            return true;
        }
    }
}

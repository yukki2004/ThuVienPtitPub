using MediatR;
using ThuVienPtit.Src.Application.Documents.Interface;
using ThuVienPtit.Src.Application.Helpers;
using ThuVienPtit.Src.Application.Interface;
using ThuVienPtit.Src.Infrastructure.Courses.Respository;

namespace ThuVienPtit.Src.Application.Documents.Command
{
    public class RestoreDocumentCommandHandle : IRequestHandler<RestoreDocumentCommand, bool>
    {
        private readonly IDocumentRespository documentRespository;
        private readonly ICacheService cacheService;
        private const string PublishVersionKey = "PublishDoc:version";
        private const string DeleteVersionKey = "DeleteDoc:version";
        private readonly Application.Courses.Interface.ICourseRespository courseRespository;

        public RestoreDocumentCommandHandle(IDocumentRespository documentRespository, ICacheService cacheService, Courses.Interface.ICourseRespository courseRespository)
        {
            this.documentRespository = documentRespository;
            this.cacheService = cacheService;
            this.courseRespository = courseRespository;
        }
        public async Task<bool> Handle(RestoreDocumentCommand command, CancellationToken cancellationToken)
        {
            var document = await documentRespository.GetDocumentByIdAsync(command.document_id);
            if (document == null)
            {
                throw new Exception("Tài liệu không tồn tại");
            }
            var newDocument = await courseRespository.GetCoursesById(document.course_id);
            if (newDocument == null)
            {
                throw new Exception("tài liệu không thuộc về khóa học nào");
            }
            if (document.status != Domain.Enum.status_document.deleted)
            {
                throw new Exception("Tài liệu không ở trạng thái đã xóa");
            }
            document.status = Domain.Enum.status_document.approved;
            await documentRespository.UpdateDocumentAsync(document, cancellationToken);
            // xử lý cache 
            // tăng version cache cho admin khi lấy các bài viết public
            var versionPub = await cacheService.GetVersionAsync(PublishVersionKey) ?? 1;
            await cacheService.SetVersionAsync(PublishVersionKey, versionPub + 1);
            // tăng version cache cho admin khi lấy các bài viết delete
            var versionDel = await cacheService.GetVersionAsync(DeleteVersionKey) ?? 1;
            await cacheService.SetVersionAsync(DeleteVersionKey, versionDel + 1);
            // tăng version cache cho user khi lọc category theo môn học
            if (!CacheVersionHelper.Map.TryGetValue(document.category, out var categorySlug))
                categorySlug = document.category;
            var categoryVersionKey = $"course:{newDocument.slug}:category:{categorySlug}:version";
            var categoryVersion = await cacheService.GetVersionAsync(categoryVersionKey) ?? 1;
            await cacheService.SetVersionAsync(categoryVersionKey, categoryVersion + 1);
            return true;
        }
    }
}

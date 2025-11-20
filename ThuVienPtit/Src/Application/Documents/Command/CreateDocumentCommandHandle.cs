using MediatR;
using ThuVienPtit.Src.Application.Documents.DTOs.Respone;
using ThuVienPtit.Src.Application.Documents.Interface;
using ThuVienPtit.Src.Application.Interface;
using ThuVienPtit.Src.Domain.Entities;
using ThuVienPtit.Src.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using ThuVienPtit.Src.Infrastructure.Data;
using ThuVienPtit.Src.Application.Helpers; 
namespace ThuVienPtit.Src.Application.Documents.Command
{
    public class CreateDocumentCommandHandle : IRequestHandler<CreateDocumentCommand, DocumentResponeDto>
    {
        private readonly IDocumentRespository documentRespository;
        private readonly IFileStorageService fileStorageService;
        private readonly ISlugDocumentService slugDocumentService;
        private readonly Application.Semesters.Interface.ISemesterRespository semesterRespository;
        private readonly Courses.Interface.ICourseRespository courseRespository;
        private readonly ICacheService cacheService;
        private readonly string baseUrl;
        private readonly string rootPath;
        public readonly AppDataContext context;

        private const string PublishVersionKey = "PublishDoc:version";
        private const string PendingVersionKey = "PendingDoc:version";

        public CreateDocumentCommandHandle(
            IDocumentRespository documentRespository,
            IFileStorageService fileStorageService,
            ISlugDocumentService slugDocumentService,
            Application.Semesters.Interface.ISemesterRespository semesterRespository,
            Courses.Interface.ICourseRespository courseRespository,
            ICacheService cacheService,
            AppDataContext context,
            IConfiguration configuration)
        {
            this.documentRespository = documentRespository;
            this.fileStorageService = fileStorageService;
            this.slugDocumentService = slugDocumentService;
            this.semesterRespository = semesterRespository;
            this.courseRespository = courseRespository;
            this.cacheService = cacheService;
            baseUrl = configuration["FileStorage:BaseUrl"] ?? throw new Exception("không tìm thấy base url");
            rootPath = configuration["FileStorage:RootPath"] ?? "";
            this.context = context;
        }

        public async Task<DocumentResponeDto> Handle(CreateDocumentCommand command, CancellationToken cancellationToken)
        {
            string avtPath = "", docPath = "";
            var semesterDoc = await semesterRespository.GetSemesterById(command.semester_id);
            if (semesterDoc == null) throw new Exception("Học kỳ không tồn tại");
            var courseDoc = await courseRespository.GetCoursesById(command.course_id);
            if (courseDoc == null) throw new Exception("Khóa học không tồn tại");
            var newDocument = new documents
            {
                document_id = Guid.NewGuid(),
                title = command.title,
                description = command.description,
                category = command.category,
                course_id = command.course_id,
                user_id = command.user_id,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
                slug = slugDocumentService.GenerateSlug(command.title),
                status = command.userRole == "admin" ? status_document.approved : status_document.pending
            };
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                (avtPath, docPath) = await fileStorageService.AddAvtDocument(
                    command.avtDocument,
                    command.fileDocument,
                    semesterDoc.name,
                    courseDoc.name,
                    command.category
                );

                newDocument.avt_document = avtPath;
                newDocument.file_path = docPath;

                await documentRespository.AddDocumentAndTagAsync(newDocument, command.tag_ids, cancellationToken);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                if (!string.IsNullOrEmpty(docPath))
                   await fileStorageService.DeleteFileDocument(docPath);

                throw;
            }
            // xử lý cache
            if (command.userRole == "admin")
            {
                // nếu là admin thì tăng version cache danh sách bài viết public
                var version = await cacheService.GetVersionAsync(PublishVersionKey) ?? 1;
                await cacheService.SetVersionAsync(PublishVersionKey, version + 1);
                // tăng version cache cho lọc document theo category của môn học khi admin thêm đúng vào thể loại đấy
                if (!CacheVersionHelper.Map.TryGetValue(newDocument.category, out var categorySlug))
                    categorySlug = newDocument.category;
                var categoryVersionKey = $"course:{courseDoc.slug}:category:{categorySlug}:version";
                var categoryVersion = await cacheService.GetVersionAsync(categoryVersionKey) ?? 1;
                await cacheService.SetVersionAsync(categoryVersionKey, categoryVersion+1);
            }
            else
            {
                // nếu là user thì bài viết ở dạng pending tăng version cache cho các bài viết pending
                var version = await cacheService.GetVersionAsync(PendingVersionKey) ?? 1;
                await cacheService.SetVersionAsync(PendingVersionKey, version + 1);
            }

            return new DocumentResponeDto
            {
                document_id = newDocument.document_id,
                title = newDocument.title,
                description = newDocument.description,
                category = newDocument.category,
                status = newDocument.status,
                course_id = newDocument.course_id,
                user_id = newDocument.user_id,
                slug = newDocument.slug,
                avt_document = Path.Combine(baseUrl, newDocument.avt_document).Replace("\\", "/"),
                file_path = Path.Combine(baseUrl, newDocument.file_path).Replace("\\", "/"),
                created_at = newDocument.created_at,
                updated_at = newDocument.updated_at,
            };
        }
    }
}

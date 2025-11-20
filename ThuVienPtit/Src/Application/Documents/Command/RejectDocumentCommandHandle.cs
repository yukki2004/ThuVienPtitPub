using MediatR;
using ThuVienPtit.Src.Application.Courses.Interface;
using ThuVienPtit.Src.Application.Documents.Interface;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Documents.Command
{
    public class RejectDocumentCommandHandle : IRequestHandler<RejectDocumentCommand, bool>
    {
        private readonly IDocumentRespository documentRespository;
        private readonly Application.Documents.Interface.IFileStorageService fileStorageService;
        private readonly ICacheService cacheService;
        private const string PendingVersionKey = "PendingDoc:version";
        public RejectDocumentCommandHandle(IDocumentRespository documentRespository, Application.Documents.Interface.IFileStorageService fileStorageService, ICacheService cacheService)
        {
            this.documentRespository = documentRespository;
            this.cacheService = cacheService;
            this.fileStorageService = fileStorageService;
        }
        public async Task<bool> Handle(RejectDocumentCommand command, CancellationToken cancellationToken)
        {
            var document = await documentRespository.GetDocumentByIdAsync(command.document_id);
            if (document == null)
            {
                throw new Exception("Tài liệu không tồn tại");
            }
            var folderPath = document.file_path;
            if (document.status != Domain.Enum.status_document.pending)
            {
                throw new Exception("Bài viết đang trong trạng thái chờ duyệt");
            }
            await fileStorageService.DeleteFileDocument(folderPath);
            await documentRespository.DeleteDocumentAsync(document.document_id, cancellationToken);
            // xử lý cache
            // tăng version cache cho admin khi lấy các bài viết pending
            var version = await cacheService.GetVersionAsync(PendingVersionKey) ?? 1;
            await cacheService.SetVersionAsync(PendingVersionKey, version + 1);
            return true;
        }
    }
}

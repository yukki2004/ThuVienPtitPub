using MediatR;
using ThuVienPtit.Src.Application.Documents.Interface;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Documents.Command
{
    public class ClearDocumentCommandHandle : IRequestHandler<ClearDocumentCommand, bool>
    {
        private readonly IDocumentRespository documentRespository;
        private readonly Application.Documents.Interface.IFileStorageService fileStorageService;
        private const string DeleteVersionKey = "DeleteDoc:version";
        private readonly ICacheService cacheService;
        public ClearDocumentCommandHandle(IDocumentRespository documentRespository, Application.Documents.Interface.IFileStorageService fileStorageService, ICacheService cacheService)
        {
            this.documentRespository = documentRespository;
            this.fileStorageService = fileStorageService;
            this.cacheService = cacheService;
        }
        public async Task<bool> Handle(ClearDocumentCommand command, CancellationToken cancellationToken)
        {
            var document = await documentRespository.GetDocumentByIdAsync(command.document_id);
            if (document == null)
            {
                throw new Exception("Tài liệu không tồn tại");
            }
            if(document.status != Domain.Enum.status_document.deleted)
            {
                throw new Exception("Chỉ có thể xóa tài liệu đã bị đánh dấu xóa");
            }
            await fileStorageService.DeleteFileDocument(document.file_path);
            await documentRespository.DeleteDocumentAsync(command.document_id, cancellationToken);
            // xử lý cache
            // khi clear hẳn bài viết thì bài viết trong list delete bị thay đổi tăng version cho các bài viết delete
            var version = await cacheService.GetVersionAsync(DeleteVersionKey) ?? 1;
            await cacheService.SetVersionAsync(DeleteVersionKey, version + 1);
            return true;
        }

    }
}

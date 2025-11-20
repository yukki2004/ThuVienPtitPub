using System.Reflection.Metadata;
using ThuVienPtit.Src.Application.Documents.DTOs.Respone;
using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Documents.Interface
{
    public interface IDocumentRespository
    {
        Task AddDocumentAsync(documents document, CancellationToken cancellationToken);
        Task UpdateDocumentAsync(documents document, CancellationToken cancellationToken);
        Task<documents?> GetDocumentByIdAsync(Guid document_id);
        Task DeleteDocumentAsync(Guid document, CancellationToken cancellationToken);
        Task AddDocumentAndTagAsync(documents document,List<int> tags, CancellationToken cancellationToken);
        Task UpdateDocumentAndTagAsync(documents document, List<int> tags, CancellationToken cancellationToken);
        Task<Application.Documents.DTOs.Respone.GetDocumentById.DocumentDto?> GetDocumentSemesterCourseTagBySlugAsync(string slug);
        Task<List<GetRelatedDocumentDto>> GetRelatedDocuments(Guid documentId, CancellationToken cancellationToken, int limit = 10);
        Task<(int pageNumber, int pageSize, int totalPage, int toTalItem, List<Application.Documents.DTOs.Respone.GetDocumets.DocumentDto>)> GetPendingDocumentAdminAsync(int pageNumber);
        Task<(int pageNumber, int pageSize, int totalPage, int toTalItem, List<Application.Documents.DTOs.Respone.GetDocumets.DocumentDto>)> GetDeleteDocumentAdminAsync(int pageNumber);
        Task<(int pageNumber, int pageSize, int totalPage, int toTalItem, List<Application.Documents.DTOs.Respone.GetDocumets.DocumentDto>)> GetPublishDocumentAdminAsync(int pageNumber);



    }
}

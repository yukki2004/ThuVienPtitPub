using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Tags.Interface
{
    public interface ITagRespository
    {
        Task AddTagAsync(Domain.Entities.tags tag);
        Task DeleteTagAsync(int tagId);
        Task<Domain.Entities.tags?> GetTagByIdAsync(int tagId);
        Task<Domain.Entities.tags?> GetTagBySlugAsync(string slug);
        Task<List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto>> GetAllTagsAsync(CancellationToken cancellationToken);
        Task<(int pageSize, int pageNumber, int totalPage, int totalItem, List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentDto>)>
                    GetDocumentByIdAsync(int tagId, int pageNumber, CancellationToken cancellationToken);
        Task<(int totalPage, int pageNumber, int pageSize, int totalItem, List<ThuVienPtit.Src.Application.Tags.DTOs.Entity.TagDto>)> GetAllListTagAdminAsync(int pageNumber);
        Task<ThuVienPtit.Src.Application.Tags.DTOs.Respone.TagStatDto?> GetTagStatAsync(int tagId);
        Task<(int pageNumber, int totalPage, int pageSize, int totalItem, List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentFilterDto>)> GetDocumentByTagFilterAsync(int tagId, int pageNumber, int? status);
    }
}

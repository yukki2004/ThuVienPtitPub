using ThuVienPtit.Src.Application.Tags.DTOs.Entity;

namespace ThuVienPtit.Src.Application.Tags.DTOs.Respone
{
    public class GetDocumentByTag
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public int totalRecords { get; set; }
        public int totalPages { get; set; } 
        public List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentDto> documents { get; set; } = new List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentDto>();
    }
}

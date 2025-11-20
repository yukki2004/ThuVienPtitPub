using ThuVienPtit.Src.Application.Documents.DTOs.EntityDto;

namespace ThuVienPtit.Src.Application.Documents.DTOs.Respone
{
    public class GetDocumentRespone
    {
        public int toTalPages { get; set; }
        public int toTalItem { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public List<Application.Documents.DTOs.Respone.GetDocumets.DocumentDto> Docs { get; set; } = new List<Application.Documents.DTOs.Respone.GetDocumets.DocumentDto>();
    }
}

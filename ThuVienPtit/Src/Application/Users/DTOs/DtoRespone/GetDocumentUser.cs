using ThuVienPtit.Src.Application.Users.DTOs.Entities;

namespace ThuVienPtit.Src.Application.Users.DTOs.DtoRespone
{
    public class GetDocumentUser
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public List<Users.DTOs.Entities.PubAndPenDocUserDTO.DocumentDto> Documents { get; set; } = new List<Entities.PubAndPenDocUserDTO.DocumentDto>();
    }
}

namespace ThuVienPtit.Src.Application.Tags.DTOs.Respone
{
    public class GetListTagAllDto
    {
        public int totalPage { get; set; }
        public int totalItem { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public List<ThuVienPtit.Src.Application.Tags.DTOs.Entity.TagDto> tags { get; set; } = new List<ThuVienPtit.Src.Application.Tags.DTOs.Entity.TagDto>();
    }
}

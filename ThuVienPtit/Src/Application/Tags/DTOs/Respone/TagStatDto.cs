namespace ThuVienPtit.Src.Application.Tags.DTOs.Respone
{
    public class TagStatDto
    {
        public ThuVienPtit.Src.Application.Tags.DTOs.Entity.TagDto tag { get; set; } = null!;
        public int approved { get; set; }
        public int deleted { get; set; }
        public int pending { get; set; }
        public int toTal {  get; set; }
    }
}

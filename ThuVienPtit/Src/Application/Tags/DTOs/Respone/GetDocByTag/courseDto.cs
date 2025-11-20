namespace ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag
{
    public class courseDto
    {
        public Guid course_id { get; set; }
        public string name { get; set; } = null!;
        public string slug { get; set; } = null!;
    }
}

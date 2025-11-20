namespace ThuVienPtit.Src.Application.Documents.DTOs.EntityDto
{
    public class SemesterDto
    {
        public Guid semester_id { get; set; }
        public string name { get; set; } = null!;
        public int year { get; set; }
    }
}

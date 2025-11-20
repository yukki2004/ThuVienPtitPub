namespace ThuVienPtit.Src.Application.Tags.DTOs.Entity
{
    public class SemesterDto
    {
        public Guid semester_id { get; set; }
        public string name { get; set; } = null!;
        public int year { get; set; }
    }
}

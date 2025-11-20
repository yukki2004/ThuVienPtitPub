namespace ThuVienPtit.Src.Application.Courses.DTOs.Entity
{
    public class SemesterDto
    {
        public Guid semester_id { get; set; }
        public string name { get; set; } = null!;
        public int year { get; set; }
    }
}

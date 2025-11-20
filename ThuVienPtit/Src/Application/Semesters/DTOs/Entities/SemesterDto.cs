namespace ThuVienPtit.Src.Application.Semesters.DTOs.Entities
{
    public class SemesterDto
    {
        public Guid semester_id { get; set; }
        public string name { get; set; } = null!;
        public int year { get; set; }
        public List<CourseDto> courses { get; set; } = null!;
    }
}

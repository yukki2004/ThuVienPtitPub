namespace ThuVienPtit.Src.Application.Semesters.DTOs.Respone.GetFullCouresSemester
{
    public class SemesterDto
    {
        public Guid semester_id { get; set; }
        public string name { get; set; } = null!;
        public List<Semesters.DTOs.Respone.GetFullCouresSemester.CourseDto> courses { get; set; } = new List<CourseDto>();
    }
}

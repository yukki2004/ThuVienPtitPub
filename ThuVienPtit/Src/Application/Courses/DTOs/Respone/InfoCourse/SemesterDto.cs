namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse
{
    public class SemesterDto
    {
        public Guid SemesterId { get; set; }
        public string Name { get; set; } = null!;
        public int Year { get; set; }
    }
}

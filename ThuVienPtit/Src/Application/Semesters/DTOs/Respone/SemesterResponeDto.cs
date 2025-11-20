namespace ThuVienPtit.Src.Application.Semesters.DTOs.Respone
{
    public class SemesterResponeDto
    {
        public Guid semester_id { get; set; }
        public string name { get; set; } = null!;
        public int year { get; set; }
    }
}

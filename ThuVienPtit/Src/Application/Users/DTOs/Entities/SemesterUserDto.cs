namespace ThuVienPtit.Src.Application.Users.DTOs.Entities
{
    public class SemesterUserDto
    {
        public Guid semester_id { get; set; }
        public string name { get; set; } = null!;
        public int year { get; set; }

    }
}

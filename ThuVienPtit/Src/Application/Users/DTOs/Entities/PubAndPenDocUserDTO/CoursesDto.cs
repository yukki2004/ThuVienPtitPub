namespace ThuVienPtit.Src.Application.Users.DTOs.Entities.PubAndPenDocUserDTO
{
    public class CoursesDto
    {
        public Guid course_id { get; set; }
        public string name { get; set; } = null!;
        public string slug { get; set; } = null!;
        public Users.DTOs.Entities.PubAndPenDocUserDTO.SemesterDto semester { get; set; } = null!;
    }
}

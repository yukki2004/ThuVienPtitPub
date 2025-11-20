namespace ThuVienPtit.Src.Application.Semesters.DTOs.Respone.GetFullCouresSemester
{
    public class CategoryCoursesDto
    {
        public int category_id { get; set; }
        public string name { get; set; } = null!;
        public string? description { get; set; } = null!;
    }
}

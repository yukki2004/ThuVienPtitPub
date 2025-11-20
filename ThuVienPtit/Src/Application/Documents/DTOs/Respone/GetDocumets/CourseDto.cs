namespace ThuVienPtit.Src.Application.Documents.DTOs.Respone.GetDocumets
{
    public class CourseDto
    {
        public Guid course_id { get; set; }
        public string name { get; set; } = null!;
        public string slug { get; set; } = null!;
        public Application.Documents.DTOs.Respone.GetDocumets.SemesterDto semester { get; set; } = null!;
    }
}

namespace ThuVienPtit.Src.Application.Documents.DTOs.Respone.GetDocumentById
{
    public class CourseDto
    {
        public Guid course_id { get; set; }
        public string name { get; set; } = null!;
        public string slug { get; set; } = null!;
        public Documents.DTOs.Respone.GetDocumentById.SemesterDto semester { get; set; } = null!;

    }
}

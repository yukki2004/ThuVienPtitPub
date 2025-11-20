namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone
{
    public class DocumentStatisticSummaryDto
    {
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Pending { get; set; }
        public int Deleted { get; set; }
        public Dictionary<string, int> ByCategory { get; set; } = new();
    }
}

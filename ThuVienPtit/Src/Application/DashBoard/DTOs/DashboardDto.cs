namespace ThuVienPtit.Src.Application.DashBoard.DTOs
{
    public class DashboardDto
    {
        public int DocumentTotal { get; set; }
        public int CourseTotal { get; set; }
        public int UserTotal { get; set; }
        public int TagTotal { get; set; }
        public DocumentStatusDto DocumentStatus { get; set; } = new();
        public List<MonthlyCountDto> DocumentMonthly { get; set; } = new();
        public List<TagDocumentCountDto> DocumentByTag { get; set; } = new();
        public List<MonthlyCountDto> UserMonthly { get; set; } = new();
    }
}

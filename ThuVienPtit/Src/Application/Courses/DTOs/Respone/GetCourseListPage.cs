namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone
{
    public class GetCourseListPage
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int tolTalPage { get; set; }
        public int tolTolItem { get; set; }
        public List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage.courseDto> coursePage { get; set; } = new List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage.courseDto>();
    }
}

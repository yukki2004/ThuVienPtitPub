
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetDocByCourseCategory;

namespace ThuVienPtit.Src.Application.Courses.DTOs.Respone
{
    public class GetDocumentRespone
    {
        public int toTalPages { get; set; }
        public int toTalItem { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public courseDto? course { get; set; }
    }
}

using MediatR;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetCourseListPageQueries : IRequest<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCourseListPage>
    {
        public int pageNumber { get; set; }
        public Guid? semester_id { get; set; }
    }
}

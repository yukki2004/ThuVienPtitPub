using MediatR;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetInfoCourseQueries : IRequest<ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse.CourseDetailDto>
    {
        public string slug { get; set; } = null!;
    }
}

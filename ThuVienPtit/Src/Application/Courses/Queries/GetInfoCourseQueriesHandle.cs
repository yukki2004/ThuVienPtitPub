using MediatR;
using ThuVienPtit.Src.Application.Courses.Interface;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetInfoCourseQueriesHandle : IRequestHandler<GetInfoCourseQueries, ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse.CourseDetailDto>
    {
        private readonly ICourseRespository courseRespository;
        public GetInfoCourseQueriesHandle(ICourseRespository courseRespository)
        {
            this.courseRespository = courseRespository;
        }
        public async Task<ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse.CourseDetailDto> Handle(GetInfoCourseQueries queries, CancellationToken cancellationToken)
        {
            var respone = await courseRespository.GetCourseBySlugAsync(queries.slug);
            if(respone == null)
            {
                return new DTOs.Respone.InfoCourse.CourseDetailDto();
            }
            return respone;

        }
    }
}

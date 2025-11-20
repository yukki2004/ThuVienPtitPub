using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;
using ThuVienPtit.Src.Application.Courses.Interface;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetCourseListPageQueriesHandle : IRequestHandler<GetCourseListPageQueries, ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCourseListPage>
    {
        private readonly ICourseRespository courseRespository;
        public GetCourseListPageQueriesHandle(ICourseRespository courseRespository)
        {
            this.courseRespository = courseRespository;
        }
        public async Task<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCourseListPage> Handle(GetCourseListPageQueries queries, CancellationToken cancellationToken)
        {
            var courseList = await courseRespository.GetCourseListPageAsync(queries.pageNumber, queries.semester_id);
            if(courseList.Item5 == null || courseList.Item5.Count() == 0)
            {
                return new GetCourseListPage
                {
                    pageNumber = queries.pageNumber,
                    pageSize = courseList.pageSize,
                    tolTalPage = courseList.toTalPage,
                    tolTolItem = courseList.tolTalItem,
                    coursePage = new List<DTOs.Respone.GetCoursePage.courseDto>()
                };
            }
            var listCourse = courseList.Item5.Select(c => new ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage.courseDto
            {
                course_id = c.course_id,
                name = c.name,
                description = c.description,
                slug = c.slug,
                credits = c.credits,
                created_at = c.created_at,
                semesterDto = new DTOs.Respone.GetCoursePage.semesterDto
                {
                    semester_id = c.semesterDto.semester_id,
                    name = c.semesterDto.name,
                },
                category_Course = new DTOs.Respone.GetCoursePage.category_course
                {
                    category_id = c.category_Course.category_id,
                    name = c.category_Course.name,
                }
            }).ToList();
            return new GetCourseListPage
            {
                tolTalPage = courseList.toTalPage,
                tolTolItem = courseList.tolTalItem,
                pageNumber = courseList.pageNumber,
                pageSize = courseList.pageSize,
                coursePage = listCourse
            };
        }
    }
}

using MediatR;

namespace ThuVienPtit.Src.Application.Semesters.Queries
{
    public class GetAllSemesterCourseCategoryQueries : IRequest<List<Application.Semesters.DTOs.Respone.GetFullCouresSemester.semester_course_category>>
    {
    }
}

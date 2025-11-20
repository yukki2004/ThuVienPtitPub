using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Entity;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetAllCourseQueries : IRequest<List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetAllCourseRespone>>
    {
    }
}

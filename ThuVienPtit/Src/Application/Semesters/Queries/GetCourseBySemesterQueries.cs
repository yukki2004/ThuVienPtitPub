using MediatR;

namespace ThuVienPtit.Src.Application.Semesters.Queries
{
    public class GetCourseBySemesterQueries : IRequest<Semesters.DTOs.Respone.GetFullCouresSemester.SemesterDto>
    {
        public Guid SemesterId { get; set; }
    }
}

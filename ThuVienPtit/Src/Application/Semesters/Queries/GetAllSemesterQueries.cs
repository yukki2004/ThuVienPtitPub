using MediatR;

namespace ThuVienPtit.Src.Application.Semesters.Queries
{
    public class GetAllSemesterQueries : IRequest<List<DTOs.Respone.SemesterResponeDto>>
    {
    }
}

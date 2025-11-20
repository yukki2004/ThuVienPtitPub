using MediatR;
using ThuVienPtit.Src.Application.Semesters.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Semesters.Command
{
    public class CreateSemesterCommand : IRequest<SemesterResponeDto>
    {
        public string name { get; set; } = null!;
        public int year { get; set; }
    }
}

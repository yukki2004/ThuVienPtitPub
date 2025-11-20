using MediatR;
using ThuVienPtit.Src.Application.Semesters.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Semesters.Command
{
    public class UpdateSemesterCommand : IRequest<SemesterResponeDto>
    {
        public Guid semester_id { get; set; }
        public string name { get; set; } = null!;
        public int year { get; set; }
    }
}

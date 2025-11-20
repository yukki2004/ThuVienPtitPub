using MediatR;

namespace ThuVienPtit.Src.Application.Semesters.Command
{
    public class DeleteSemesterCommand : IRequest<bool>
    {
        public Guid semester_id { get; set; }
    }
}

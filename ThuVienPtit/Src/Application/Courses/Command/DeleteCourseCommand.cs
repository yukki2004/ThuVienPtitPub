using MediatR;

namespace ThuVienPtit.Src.Application.Courses.Command
{
    public class DeleteCourseCommand : IRequest<bool>
    {
        public Guid CourseId { get; set; }
    }
}

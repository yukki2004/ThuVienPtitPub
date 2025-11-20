using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Courses.Command
{
    public class CreateCourseCommand : IRequest<CreateCourseResponeDto>
    {
        public string name { get; set; } = null!;
        public string? description { get; set; }
        public int credits { get; set; }
        public Guid semester_id { get; set; }
        public int category_id { get; set; }
    }
}

using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Courses.Command
{
    public class UpdateCourseCommand : IRequest<UpdateCourseCommandDto>
    {
        public Guid course_id { get; set; }
        public string? description { get; set; }
        public int credits { get; set; }
        public int category_id { get; set; }
    }
}

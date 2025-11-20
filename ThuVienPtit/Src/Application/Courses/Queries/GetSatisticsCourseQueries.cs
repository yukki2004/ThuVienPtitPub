using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetSatisticsCourseQueries : IRequest<DocumentStatisticSummaryDto>
    {
        public Guid courseid { get; set; }
    }
}

using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;
using ThuVienPtit.Src.Application.Courses.Interface;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetSatisticsCourseQueriesHandle : IRequestHandler<GetSatisticsCourseQueries, DocumentStatisticSummaryDto>
    {
        private readonly ICourseRespository courseRespository;
        public GetSatisticsCourseQueriesHandle(ICourseRespository courseRespository)
        {
            this.courseRespository = courseRespository;
        }
        public async Task<DocumentStatisticSummaryDto> Handle(GetSatisticsCourseQueries queries, CancellationToken cancellationToken)
        {
            var respone = await courseRespository.GetCourseDocumentStatisticsAsync(queries.courseid);
            return respone;
        }
    }
}

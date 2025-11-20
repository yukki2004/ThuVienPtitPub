using Azure.Core;
using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;
using ThuVienPtit.Src.Application.Courses.Interface;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetAllDocumentByCourseQueriesHandle : IRequestHandler<GetAllDocumentByCourseQueries, GetDocumentsByCourseResultDto>
    {
        private readonly ICourseRespository courseRespository;
        private readonly string baseUrl;

        public GetAllDocumentByCourseQueriesHandle(ICourseRespository courseRespository, IConfiguration configuration)
        {
            this.courseRespository = courseRespository;
            baseUrl = configuration["FileStorage:BaseUrl"] ?? "";
        }

        public async Task<GetDocumentsByCourseResultDto> Handle(GetAllDocumentByCourseQueries request, CancellationToken cancellationToken)
        {
            var (items, totalItem) = await courseRespository.GetDocumentsByCourseAsync(request.course_id, request.pageNumber, 10, request.category, request.status);
            var totalPage = (int)Math.Ceiling(totalItem / 10.0);

            var documentsDto = items.Select(d => new ThuVienPtit.Src.Application.Courses.DTOs.Entity.DocumentDto
            {
                DocumentId = d.document_id,
                Title = d.title,
                Category = d.category,
                Status = d.status,
                Slug = d.slug,
                AvtUrl = $"{baseUrl}/{d.avt_document}",
                CreatedAt = d.created_at
            }).ToList();

            return new GetDocumentsByCourseResultDto
            {
                PageNumber = request.pageNumber,
                PageSize = 10,
                TotalItem = totalItem,
                TotalPage = totalPage,
                Items = documentsDto
            };
        }
    }
}

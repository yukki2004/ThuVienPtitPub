using MediatR;
using ThuVienPtit.Src.Application.Tags.DTOs.Respone;
using ThuVienPtit.Src.Application.Tags.Interface;

namespace ThuVienPtit.Src.Application.Tags.Queries
{
    public class GetDocumentByTagQueriesHandle : IRequestHandler<GetDocumentByTagQueries, GetDocumentByTag>
    {
        private readonly ITagRespository tagRespository;
        private readonly string baseUrl;
        public GetDocumentByTagQueriesHandle(ITagRespository tagRespository, IConfiguration configuration)
        {
            this.tagRespository = tagRespository;
            this.baseUrl = configuration["FileStorage:BaseUrl"] ?? throw new ArgumentNullException(nameof(configuration), "App:BaseUrl is not configured.");
        }
        public async Task<GetDocumentByTag> Handle(GetDocumentByTagQueries request, CancellationToken cancellationToken)
        {
            var tag = await tagRespository.GetTagBySlugAsync(request.slug);
            if (tag == null)
            {
                throw new Exception("không tìm thấy tag");
            }
            var documents = await tagRespository.GetDocumentByIdAsync(tag.tag_id, request.pageNumber, cancellationToken);
            if(documents.Item5 == null)
            {
                throw new Exception("không tìm thấy document");
            }
            var respone = new GetDocumentByTag
            {
                pageNumber = request.pageNumber,
                pageSize = 10,
                totalRecords = documents.totalItem,
                totalPages = documents.totalPage,
                documents = documents.Item5.Select(d => new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentDto
                {
                    document_id = d.document_id,
                    title = d.title,
                    description = d.description,
                    avt_document = string.IsNullOrEmpty(d.avt_document) ? "" : $"{baseUrl}/{d.avt_document}",
                    created_at = d.created_at,
                    slug = d.slug,
                    course = new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.courseDto
                    {
                        course_id = d.course.course_id,
                        name = d.course.name,
                        slug = d.course.slug,
                    },
                    tags = d.tags.Select(dt => new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto
                    {
                        tag_id = dt.tag_id,
                        name = dt.name,
                        slug = dt.slug,
                    }).ToList()
                }).ToList()
            };
            return respone;
        }
    }
}

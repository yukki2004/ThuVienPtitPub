using MediatR;
using ThuVienPtit.Src.Application.Tags.DTOs.Respone;
using ThuVienPtit.Src.Application.Tags.Interface;

namespace ThuVienPtit.Src.Application.Tags.Queries
{
    public class GetTagDocumentPageFitlerQueriesHandle : IRequestHandler<GetTagDocumentPageFitlerQueries, ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocumentByTagFilterDto>
    {
        private readonly ITagRespository tagRespository;
        private string basUrl;
        public GetTagDocumentPageFitlerQueriesHandle(ITagRespository tagRespository, IConfiguration configuration)
        {
            this.tagRespository = tagRespository;
            basUrl = configuration["FileStorage:BaseUrl"] ?? " ";
        }
        public async Task<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocumentByTagFilterDto> Handle(GetTagDocumentPageFitlerQueries queries, CancellationToken cancellationToken)
        {
            var data = await tagRespository.GetDocumentByTagFilterAsync(queries.tagId, queries.pageNumber, queries.status);
            var respone = new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocumentByTagFilterDto
            {
                pageNumber = queries.pageNumber,
                totalPages = data.totalPage,
                pageSize = data.pageSize,
                totalRecords = data.totalItem,
                documents = data.Item5.Select(d => new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentFilterDto
                {
                    document_id = d.document_id,
                    title = d.title,
                    description = d.description,
                    slug = d.slug,
                    avt_document = Path.Combine(basUrl,d.avt_document).Replace("\\","/"),
                    created_at = d.created_at,
                    status = d.status,
                }).ToList(),
            };
            return respone;
        }
    }
}

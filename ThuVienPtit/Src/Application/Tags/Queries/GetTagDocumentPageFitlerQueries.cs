using MediatR;
using ThuVienPtit.Src.Application.Tags.DTOs.Respone;

namespace ThuVienPtit.Src.Application.Tags.Queries
{
    public class GetTagDocumentPageFitlerQueries : IRequest<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocumentByTagFilterDto>
    {
        public int tagId { get; set; }
        public int pageNumber { get; set; }
        public int? status { get; set; }
    }
}

using MediatR;
using ThuVienPtit.Src.Application.Documents.DTOs.Respone;
using ThuVienPtit.Src.Application.Documents.Interface;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Documents.Queries
{
    public class GetRelatedDocumentQueriesHandle : IRequestHandler<GetRelatedDocumentQueries, List<GetRelatedDocumentDto>>
    {
        private readonly IDocumentRespository documentRespository;
        private readonly string baseUrl;
        private readonly ICacheService _cacheService;
        public GetRelatedDocumentQueriesHandle(IDocumentRespository documentRespository, IConfiguration configuration, ICacheService cacheService)
        {
            this.documentRespository = documentRespository;
            this.baseUrl = configuration["FileStorage:BaseUrl"] ?? throw new Exception("không tìm thấy đường dẫn gốc");
            _cacheService = cacheService;
        }
        public async Task<List<GetRelatedDocumentDto>> Handle(GetRelatedDocumentQueries queries, CancellationToken cancellationToken)
        {
            var cacheKey = $"RelatedDocuments:{queries.document_id}";

            var cached = await _cacheService.GetAsync<List<GetRelatedDocumentDto>>(cacheKey);
            if (cached != null)
            {
                Console.WriteLine("lấy dữ liệu từ cache");
                return cached;
            }

            var relatedDocuments = await documentRespository.GetRelatedDocuments(queries.document_id,cancellationToken);

            var RelatedDocuments = relatedDocuments.Select(doc => new DTOs.Respone.GetRelatedDocumentDto
            {
                document_id = doc.document_id,
                title = doc.title,
                description = doc.description,
                category = doc.category,
                status = doc.status,
                avt_document = Path.Combine(baseUrl, doc.avt_document).Replace("\\", "/"),
                slug = doc.slug,
                created_at = doc.created_at,
            }).ToList();
            await _cacheService.SetAsync(cacheKey, RelatedDocuments, TimeSpan.FromMinutes(5));
            return RelatedDocuments;
        }
    }
}

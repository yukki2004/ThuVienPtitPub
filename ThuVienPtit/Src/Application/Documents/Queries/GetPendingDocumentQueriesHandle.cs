using MediatR;
using System.Buffers.Text;
using ThuVienPtit.Src.Application.Documents.DTOs.EntityDto;
using ThuVienPtit.Src.Application.Documents.DTOs.Respone;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Documents.Queries
{
    public class GetPendingDocumentQueriesHandle : IRequestHandler<GetPendingDocumentQueries, GetDocumentRespone>
    {
        private readonly Interface.IDocumentRespository documentRespository;
        private readonly string basUrl;
        private readonly ICacheService cacheService;
        private const string VersionKey = "PendingDoc:version";

        public GetPendingDocumentQueriesHandle(Interface.IDocumentRespository documentRespository, IConfiguration configuration, ICacheService cacheService)
        {
            this.documentRespository = documentRespository;
            basUrl = configuration["FileStorage:BaseUrl"] ?? "";
            this.cacheService = cacheService;
        }
        public async Task<GetDocumentRespone> Handle(GetPendingDocumentQueries request, CancellationToken cancellationToken)
        {
            var version = await cacheService.GetVersionAsync(VersionKey) ?? 1;
            var cacheKey = $"PendingDoc:{version}:page:{request.pageNumber}";
            var cacheData = await cacheService.GetAsync<GetDocumentRespone>(cacheKey);
            if(cacheData != null)
            {
                Console.WriteLine("Lấy dữ liệu từ Redis Cache");
                return cacheData;
            }

            var document = await documentRespository.GetPendingDocumentAdminAsync(request.pageNumber);
            if (document.Item5 == null || document.Item5.Count == 0)
            {
                return new GetDocumentRespone
                {
                    toTalItem = document.toTalItem,
                    toTalPages = document.totalPage,
                    pageSize = 0,
                    pageNumber = request.pageNumber,
                    Docs = new List<Application.Documents.DTOs.Respone.GetDocumets.DocumentDto>(),
                };

            }
            var documentRespone = new GetDocumentRespone
            {
                toTalItem = document.toTalItem,
                toTalPages = document.totalPage,
                pageSize = document.pageSize,
                pageNumber = document.pageNumber,
                Docs = document.Item5.Select(d => new DTOs.Respone.GetDocumets.DocumentDto
                {

                    document_id = d.document_id,
                    description = d.description,
                    avt_document = (d.avt_document != null) ? Path.Combine(basUrl, d.avt_document).Replace("\\", "/") : string.Empty,
                    title = d.title,
                    created_at = d.created_at,
                    slug = d.slug,
                    course = new DTOs.Respone.GetDocumets.CourseDto
                    {
                        course_id = d.course.course_id,
                        name = d.course.name,
                        slug = d.course.slug,
                        semester = new DTOs.Respone.GetDocumets.SemesterDto
                        {
                            semester_id = d.course.semester.semester_id,
                            name = d.course.semester.name
                        }
                    },
                    user = new DTOs.Respone.GetDocumets.UserDto
                    {
                        name = (d.user != null) ? d.user.name : string.Empty,
                        avt = (d.user != null) ? ((d.user.avt != null) ? Path.Combine(basUrl, d.user.avt).Replace("\\", "/") : string.Empty) : string.Empty,
                        user_id = (d.user != null) ? d.user.user_id : Guid.Empty,
                    },
                    tags = d.tags.Select(dt => new DTOs.Respone.GetDocumets.TagDto
                    {
                        tag_id = dt.tag_id,
                        name = dt.name,
                        slug = dt.slug,
                    }).ToList()

                }).ToList(),
            };
            await cacheService.SetAsync(cacheKey, documentRespone, TimeSpan.FromMinutes(10));
            return documentRespone;

        }
    }
}

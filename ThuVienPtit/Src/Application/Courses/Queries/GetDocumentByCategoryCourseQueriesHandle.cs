using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;
using ThuVienPtit.Src.Application.Courses.Interface;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Courses.Queries
{
    public class GetDocumentByCategoryCourseQueriesHandle : IRequestHandler<GetDocumentByCategoryCourseQueries, GetDocumentRespone>
    {
        private readonly ICourseRespository courseRespository;
        private readonly string basUrl;
        private readonly ICacheService cacheService;
        public GetDocumentByCategoryCourseQueriesHandle(ICourseRespository courseRespository, IConfiguration configuration, ICacheService cacheService)
        {
            this.courseRespository = courseRespository;
            basUrl = configuration["FileStorage:BaseUrl"] ?? "";
            this.cacheService = cacheService;
        }
        public async Task<GetDocumentRespone> Handle(GetDocumentByCategoryCourseQueries queries, CancellationToken cancellationToken)
        {
            var categoryMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "giao-trinh", "Giáo trình" },
                { "slide", "Slide" },
                { "de-thi", "Đề thi" },
                { "tai-lieu-khac", "Tài liệu khác" }
            };
            var categoryName = categoryMap.ContainsKey(queries.category) ? categoryMap[queries.category] : queries.category;
            var courseVersionKey = $"course:{queries.slug}:version";
            var categoryVersionKey = $"course:{queries.slug}:category:{queries.category}:version";
            var courseVersion = await cacheService.GetVersionAsync(courseVersionKey) ?? 1;
            var categoryVersion = await cacheService.GetVersionAsync(categoryVersionKey) ?? 1;
            var cacheKey = $"course:{queries.slug}:category:{queries.category}:page:{queries.pageNumber}:v{courseVersion}-{categoryVersion}";
            var cacheData = await cacheService.GetAsync<GetDocumentRespone>(cacheKey);
            if(cacheData != null)
            {
                return cacheData;
            }

            var document = await courseRespository.GetCategoryDocumentCourseAsync(queries.pageNumber, categoryName, queries.slug, cancellationToken);
            if (document.Item6 == null || document.Item6.Count == 0)
            {
                return new GetDocumentRespone
                {
                    toTalItem = document.toTalItem,
                    toTalPages = document.totalPage,
                    pageSize = 0,
                    pageNumber = queries.pageNumber,
                    course = null,
                };

            }
            var documentRespone = new GetDocumentRespone
            {
                toTalItem = document.toTalItem,
                toTalPages = document.totalPage,
                pageSize = document.pageSize,
                pageNumber = document.pageNumber,
                course = new DTOs.Respone.GetDocByCourseCategory.courseDto
                {
                    course_id = document.courseDto.course_id,
                    name = document.courseDto.name,
                    slug = document.courseDto.slug,
                    credits = document.courseDto.credits,
                    description = document.courseDto.description,
                    documents = document.Item6.Select(dt => new DTOs.Respone.GetDocByCourseCategory.documentDto
                    {
                        document_id = dt.document_id,
                        avt_document = dt.avt_document != null ? Path.Combine(basUrl,dt.avt_document).Replace("\\","/") : string.Empty,
                        slug = dt.slug,
                        title = dt.title,
                        description = dt.description,
                        created_at = dt.created_at,
                        tags = dt.tags.Select(tg => new DTOs.Respone.GetDocByCourseCategory.TagDocDto
                        {
                            tag_id = tg.tag_id,
                            name = tg.name,
                            slug = tg.slug,
                        }).ToList()
                    }).ToList(),
                }
            };
            await cacheService.SetAsync(cacheKey, documentRespone, TimeSpan.FromMinutes(50));
            return documentRespone;

        }
    }
}

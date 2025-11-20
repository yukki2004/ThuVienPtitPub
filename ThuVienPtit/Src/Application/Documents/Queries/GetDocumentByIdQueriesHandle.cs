using MediatR;
using ThuVienPtit.Src.Application.Documents.DTOs.Respone;
using ThuVienPtit.Src.Application.Documents.Interface;
using ThuVienPtit.Src.Application.Interface;

namespace ThuVienPtit.Src.Application.Documents.Queries
{
    public class GetDocumentByIdQueriesHandle : IRequestHandler<GetDocumentByIdQueries, GetDocumentByIdQueriesDto>
    {
        private readonly IDocumentRespository documentRespository;
        private readonly string baseUrl;
        private readonly ICacheService cacheService;
        public GetDocumentByIdQueriesHandle(IDocumentRespository documentRespository, IConfiguration configuration, ICacheService cacheService)
        {
            this.documentRespository = documentRespository;
            baseUrl = configuration["FileStorage:BaseUrl"] ?? throw new Exception("không tìm thấy base url");
            this.cacheService = cacheService;
        }
        public async Task<GetDocumentByIdQueriesDto> Handle(GetDocumentByIdQueries queries, CancellationToken cancellationToken)
        {

            if(queries.userRole != "admin")
            {
                var cacheKey = $"ViewDoc:Slug:{queries.slug}";
                var cacheData = await cacheService.GetAsync<GetDocumentByIdQueriesDto>(cacheKey);
                if (cacheData != null)
                {
                    return cacheData;
                }
                var document = await documentRespository.GetDocumentSemesterCourseTagBySlugAsync(queries.slug);
                if (document == null)
                {
                    throw new Exception("lỗi không tìm thấy dữ liệu trong database");
                }
                if (document.status == Domain.Enum.status_document.pending && (document.user_id != queries.user_id || document.user_id == null || queries.user_id == null))
                {
                    throw new Exception("bạn không được quyền xem bài viết này");
                }
                if(document.status == Domain.Enum.status_document.deleted)
                {
                    throw new Exception("bài viết đã bị xóa");
                }
                var respone = new GetDocumentByIdQueriesDto
                {
                    DocDto = new Documents.DTOs.Respone.GetDocumentById.DocumentDto
                    {
                        document_id = document.document_id,
                        title = document.title,
                        description = document.description,
                        file_path = Path.Combine(baseUrl, document.file_path).Replace("\\", "/"),
                        avt_document = Path.Combine(baseUrl, document.avt_document).Replace("\\", "/"),
                        slug = document.slug,
                        status = document.status,
                        created_at = document.created_at,
                        course = new Documents.DTOs.Respone.GetDocumentById.CourseDto
                        {
                            course_id = document.course.course_id,
                            name = document.course.name,
                            semester = new Documents.DTOs.Respone.GetDocumentById.SemesterDto
                            {
                                semester_id = document.course.semester.semester_id,
                                name = document.course.semester.name,
                            },
                        },
                        tags = document.tags.Select(dt => new Documents.DTOs.Respone.GetDocumentById.TagDto
                        {
                            tag_id = dt.tag_id,
                            name = dt.name,
                            slug = dt.slug,
                        }).ToList()
                    }
                };
                
                if (document.status == Domain.Enum.status_document.approved)
                {
                    await cacheService.SetAsync(cacheKey, respone, TimeSpan.FromMinutes(50));
                }
                return respone;
            }
            else
            {
                var document = await documentRespository.GetDocumentSemesterCourseTagBySlugAsync(queries.slug);
                if (document == null)
                {
                    throw new Exception("lỗi không tìm thấy dữ liệu trong database");
                }
                var respone = new GetDocumentByIdQueriesDto
                {
                    DocDto = new Documents.DTOs.Respone.GetDocumentById.DocumentDto
                    {
                        document_id = document.document_id,
                        title = document.title,
                        description = document.description,
                        file_path = Path.Combine(baseUrl, document.file_path).Replace("\\", "/"),
                        avt_document = Path.Combine(baseUrl, document.avt_document).Replace("\\", "/"),
                        slug = document.slug,
                        status = document.status,
                        created_at = document.created_at,
                        course = new Documents.DTOs.Respone.GetDocumentById.CourseDto
                        {
                            course_id = document.course.course_id,
                            name = document.course.name,
                            semester = new Documents.DTOs.Respone.GetDocumentById.SemesterDto
                            {
                                semester_id = document.course.semester.semester_id,
                                name = document.course.semester.name,
                            },
                        },
                        tags = document.tags.Select(dt => new Documents.DTOs.Respone.GetDocumentById.TagDto
                        {
                            tag_id = dt.tag_id,
                            name = dt.name,
                            slug = dt.slug,
                        }).ToList()
                    }
                };
                return respone;
            }
           
        }
    }
}

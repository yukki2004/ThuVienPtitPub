using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using ThuVienPtit.Src.Application.Documents.DTOs.Respone;
using ThuVienPtit.Src.Application.Documents.Interface;
using ThuVienPtit.Src.Domain.Entities;
using ThuVienPtit.Src.Infrastructure.Data;

namespace ThuVienPtit.Src.Infrastructure.Documents.Respository
{
    public class DocumentRespository : IDocumentRespository
    {
        private readonly AppDataContext _context;
        public DocumentRespository(AppDataContext context)
        {
            _context = context;
        }
        public async Task AddDocumentAsync(documents document, CancellationToken cancellationToken)
        {
            await _context.documents.AddAsync(document);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateDocumentAsync(documents document, CancellationToken cancellationToken)
        {
            _context.documents.Update(document);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<documents?> GetDocumentByIdAsync(Guid document_id)
        {
            return await _context.documents.FindAsync(document_id);
        }
        public async Task DeleteDocumentAsync(Guid document_id, CancellationToken cancellationToken)
        {
            var document = await _context.documents.FindAsync(document_id);
            if (document != null)
            {
                _context.documents.Remove(document);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task AddDocumentAndTagAsync(documents document, List<int> tags, CancellationToken cancellationToken)
        {
            await _context.documents.AddAsync(document);
            foreach (var tagId in tags)
            {
                var documentTag = new document_tags
                {
                    document_id = document.document_id,
                    tag_id = tagId
                };
                await _context.document_tags.AddAsync(documentTag);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateDocumentAndTagAsync(documents document, List<int> tags, CancellationToken cancellationToken)
        {
            _context.documents.Update(document);
            var existingTags = _context.document_tags.Where(dt => dt.document_id == document.document_id);
            _context.document_tags.RemoveRange(existingTags);
            foreach (var tagId in tags)
            {
                var documentTag = new document_tags
                {
                    document_id = document.document_id,
                    tag_id = tagId
                };
                await _context.document_tags.AddAsync(documentTag);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<Application.Documents.DTOs.Respone.GetDocumentById.DocumentDto?> GetDocumentSemesterCourseTagBySlugAsync(string slug)
        {
            var respone = await _context.documents
                .Where(dt => dt.slug == slug)
                .Select(dt=> new Application.Documents.DTOs.Respone.GetDocumentById.DocumentDto
                {
                    document_id = dt.document_id,
                    avt_document = dt.avt_document,
                    slug = dt.slug,
                    file_path = dt.file_path,
                    created_at = dt.created_at,
                    title = dt.title,
                    status = dt.status,
                    user_id = dt.user_id,
                    description = dt.description,
                    tags = dt.document_tags.Select(dtg => new Application.Documents.DTOs.Respone.GetDocumentById.TagDto
                    {
                        tag_id = dtg.tag_id,
                        name = dtg.Tag.name,
                        slug = dtg.Tag.slug,

                    }).ToList(),
                    course = new Application.Documents.DTOs.Respone.GetDocumentById.CourseDto
                    {
                        course_id = dt.course_id,
                        name = dt.course.name,
                        slug = dt.course.slug,
                        semester = new Application.Documents.DTOs.Respone.GetDocumentById.SemesterDto
                        {
                            semester_id = dt.course.semester.semester_id,
                            name = dt.course.semester.name
                        }
                    }
                })
                .FirstOrDefaultAsync();
            return respone;
        }
        public async Task<List<GetRelatedDocumentDto>> GetRelatedDocuments(Guid documentId, CancellationToken cancellationToken, int limit = 10)
        {
            var tagIds = await _context.document_tags
                .Where(dt => dt.document_id == documentId)
                .Select(dt => dt.tag_id)
                .ToListAsync();
            if (tagIds.Count == 0)
                return new List<GetRelatedDocumentDto>();
            var relatedDocuments = await (
                from d in _context.documents
                join dt in _context.document_tags on d.document_id equals dt.document_id
                where tagIds.Contains(dt.tag_id)
                      && d.document_id != documentId
                      && d.status == Domain.Enum.status_document.approved
                group dt by new
                {
                    d.document_id,
                    d.avt_document,
                    d.title,
                    d.description,
                    d.created_at,
                    d.slug,
                    d.status,
                    d.category
                }
                into g
                orderby g.Count() descending
                select new GetRelatedDocumentDto
                {
                    document_id = g.Key.document_id,
                    avt_document = g.Key.avt_document,
                    title = g.Key.title,
                    description = g.Key.description,
                    created_at = g.Key.created_at,
                    slug = g.Key.slug,
                    status = g.Key.status,
                    category = g.Key.category
                })
                .Take(limit)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return relatedDocuments;
        }

        public async Task<(int pageNumber, int pageSize, int totalPage, int toTalItem, List<Application.Documents.DTOs.Respone.GetDocumets.DocumentDto>)> GetPendingDocumentAdminAsync(int pageNumber)
        {
            var query = _context.documents
                .AsNoTracking()
                .Where(d => d.status == Domain.Enum.status_document.pending);
            var totalItems = await query.CountAsync();
            var documents = await _context.documents
                .Where(d => d.status == Domain.Enum.status_document.pending)
                .OrderByDescending(d => d.created_at)
                .AsNoTracking()
                .Select(d => new Application.Documents.DTOs.Respone.GetDocumets.DocumentDto
                {
                    document_id = d.document_id,
                    avt_document = d.avt_document,
                    created_at = d.created_at,
                    slug = d.slug,
                    title = d.title,
                    description = d.description,
                    course = new Application.Documents.DTOs.Respone.GetDocumets.CourseDto
                    {
                        course_id = d.course_id,
                        name = d.course.name,
                        slug = d.course.slug,
                        semester = new Application.Documents.DTOs.Respone.GetDocumets.SemesterDto
                        {
                            semester_id = d.course.semester.semester_id,
                            name = d.course.semester.name
                        }
                    },
                    tags = d.document_tags.Select(dt => new Application.Documents.DTOs.Respone.GetDocumets.TagDto
                    {
                        tag_id = dt.tag_id,
                        slug = dt.Tag.slug,
                        name = dt.Tag.name,
                    }).ToList(),
                    user = (d.user != null) ? new Application.Documents.DTOs.Respone.GetDocumets.UserDto
                    {
                        user_id = d.user.user_id,
                        name = d.user.name,
                        avt = d.user.img,
                    } : null,

                })
                .Skip((pageNumber - 1) * 10)
                .Take(10)
                .ToListAsync();
            var pageSize = 10;
            var pageNumbers = pageNumber;
            var tolTalPage = (int)Math.Ceiling((double)totalItems / 10);
            return (pageNumbers, pageSize, tolTalPage, totalItems, documents);

        }
        public async Task<(int pageNumber, int pageSize, int totalPage, int toTalItem, List<Application.Documents.DTOs.Respone.GetDocumets.DocumentDto>)> GetDeleteDocumentAdminAsync(int pageNumber)
        {
            var query = _context.documents
                .AsNoTracking()
                .Where(d => d.status == Domain.Enum.status_document.deleted);
            var totalItems = await query.CountAsync();
            var documents = await _context.documents
                .Where(d => d.status == Domain.Enum.status_document.deleted)
                .OrderByDescending(d => d.created_at)
                .AsNoTracking()
                .Select(d => new Application.Documents.DTOs.Respone.GetDocumets.DocumentDto
                {
                    document_id = d.document_id,
                    avt_document = d.avt_document,
                    created_at = d.created_at,
                    slug = d.slug,
                    title = d.title,
                    description = d.description,
                    course = new Application.Documents.DTOs.Respone.GetDocumets.CourseDto
                    {
                        course_id = d.course_id,
                        name = d.course.name,
                        slug = d.course.slug,
                        semester = new Application.Documents.DTOs.Respone.GetDocumets.SemesterDto
                        {
                            semester_id = d.course.semester.semester_id,
                            name = d.course.semester.name
                        }
                    },
                    tags = d.document_tags.Select(dt => new Application.Documents.DTOs.Respone.GetDocumets.TagDto
                    {
                        tag_id = dt.tag_id,
                        slug = dt.Tag.slug,
                        name = dt.Tag.name,
                    }).ToList(),
                    user = (d.user != null) ? new Application.Documents.DTOs.Respone.GetDocumets.UserDto
                    {
                        user_id = d.user.user_id,
                        name = d.user.name,
                        avt = d.user.img,
                    } : null,

                })
                .Skip((pageNumber - 1) * 10)
                .Take(10)
                .ToListAsync();
            var pageSize = 10;
            var pageNumbers = pageNumber;
            var tolTalPage = (int)Math.Ceiling((double)totalItems / 10);
            return (pageNumbers, pageSize, tolTalPage, totalItems, documents);

        }
        public async Task<(int pageNumber, int pageSize, int totalPage, int toTalItem, List<Application.Documents.DTOs.Respone.GetDocumets.DocumentDto>)> GetPublishDocumentAdminAsync(int pageNumber)
        {
            var query = _context.documents
                .AsNoTracking()
                .Where(d => d.status == Domain.Enum.status_document.approved);
            var totalItems = await query.CountAsync();
            var documents = await _context.documents
                .Where(d => d.status == Domain.Enum.status_document.approved)
                .OrderByDescending(d => d.created_at)
                .AsNoTracking()
                .Select(d => new Application.Documents.DTOs.Respone.GetDocumets.DocumentDto
                {
                    document_id = d.document_id,
                    avt_document = d.avt_document,
                    created_at = d.created_at,
                    slug = d.slug,
                    title = d.title,
                    description = d.description,
                    course = new Application.Documents.DTOs.Respone.GetDocumets.CourseDto
                    {
                        course_id = d.course_id,
                        name = d.course.name,
                        slug = d.course.slug,
                        semester = new Application.Documents.DTOs.Respone.GetDocumets.SemesterDto
                        {
                            semester_id = d.course.semester.semester_id,
                            name = d.course.semester.name
                        }
                    },
                    tags = d.document_tags.Select(dt => new Application.Documents.DTOs.Respone.GetDocumets.TagDto
                    {
                        tag_id = dt.tag_id,
                        slug = dt.Tag.slug,
                        name = dt.Tag.name,
                    }).ToList(),
                    user = (d.user != null) ? new Application.Documents.DTOs.Respone.GetDocumets.UserDto
                    {
                        user_id = d.user.user_id,
                        name = d.user.name,
                        avt = d.user.img,
                    } : null,

                })
                .Skip((pageNumber - 1) * 10)
                .Take(10)
                .ToListAsync();
            var pageSize = 10;
            var pageNumbers = pageNumber;
            var tolTalPage = (int)Math.Ceiling((double)totalItems / 10);
            return (pageNumbers, pageSize, tolTalPage, totalItems, documents);

        }
    }
}

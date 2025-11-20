using Microsoft.EntityFrameworkCore;
using ThuVienPtit.Src.Application.Tags.Interface;
using ThuVienPtit.Src.Domain.Entities;
using ThuVienPtit.Src.Domain.Enum;
using ThuVienPtit.Src.Infrastructure.Data;

namespace ThuVienPtit.Src.Infrastructure.Tags.Respository
{
    public class TagRespository : ITagRespository
    {
        private readonly AppDataContext _context;
        public TagRespository(AppDataContext context)
        {
            _context = context;
        }
        public async Task AddTagAsync(Domain.Entities.tags tag)
        {
            await _context.tags.AddAsync(tag);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTagAsync(int tagId)
        {
            var tag =  await _context.tags.FirstOrDefaultAsync(t => t.tag_id == tagId);
            if (tag != null)
            {
                 _context.tags.Remove(tag);
            }
            await _context.SaveChangesAsync();
        }
        public async Task<Domain.Entities.tags?> GetTagByIdAsync(int tagId)
        {
            var tag = await _context.tags.FindAsync(tagId);
            return tag;
        }
        public async Task<List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto>> GetAllTagsAsync(CancellationToken cancellationToken)
        {
            var respone = await _context.tags
                .AsNoTracking()
                .Select(tg => new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto
                {
                    tag_id = tg.tag_id,
                    name = tg.name,
                    slug = tg.slug,
                }).ToListAsync(cancellationToken);
            return respone;
        }
        public async Task<(int pageSize, int pageNumber, int totalPage, int totalItem, List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentDto>)>
            GetDocumentByIdAsync(int tagId, int pageNumber, CancellationToken cancellationToken)
        {
            const int pageSize = 10;

            var baseQuery = _context.documents
                .AsNoTracking()
                .Where(d => d.status == status_document.approved && d.document_tags.Any(dt => dt.tag_id == tagId));

            var totalItem = await baseQuery.CountAsync(cancellationToken);

            if (totalItem == 0)
            {
                return (pageSize, pageNumber, 0, 0, new List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentDto>());
            }
            var documents = await baseQuery
                .OrderByDescending(d => d.created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentDto
                {
                    document_id = d.document_id,
                    title = d.title,
                    description = d.description,
                    avt_document = d.avt_document,
                    created_at = d.created_at,
                    slug = d.slug,
                    course = new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.courseDto
                    {
                        course_id = d.course.course_id,
                        name = d.course.name,
                        slug = d.course.slug,
                    },
                    tags = d.document_tags.Select(dt => new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.tagDto
                    {
                        tag_id = dt.Tag.tag_id,
                        name = dt.Tag.name,
                        slug = dt.Tag.slug,
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            var totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

            return (pageSize, pageNumber, totalPage, totalItem, documents);
        }

        public async Task<tags?> GetTagBySlugAsync(string slug)
        {
            var tag = await _context.tags.FirstOrDefaultAsync(t => t.slug == slug);
            return tag;
        }
        public async Task<(int totalPage, int pageNumber, int pageSize, int totalItem, List<ThuVienPtit.Src.Application.Tags.DTOs.Entity.TagDto>)> GetAllListTagAdminAsync(int pageNumber)
        {
            var totalItem = await _context.tags.CountAsync();
            var pageSize = 10;
            var Tags = await _context.tags
                .AsNoTracking()
                .OrderByDescending(t=>t.created_at)
                .Skip((pageNumber -1)*pageSize)
                .Take(pageSize)
                .Select(t=> new ThuVienPtit.Src.Application.Tags.DTOs.Entity.TagDto
                {
                    tag_id = t.tag_id,
                    name = t.name,
                    slug=t.slug,
                    description = t.description,
                    created_at  = t.created_at,
                })
                .ToListAsync();
            var tolTalPage = (int)Math.Ceiling((double)totalItem / pageSize);
            return (tolTalPage, pageNumber, pageSize, totalItem, Tags);
        }
        public async Task<ThuVienPtit.Src.Application.Tags.DTOs.Respone.TagStatDto?> GetTagStatAsync(int tagId)
        {
            var query = _context.document_tags
                    .Where(dt => dt.tag_id == tagId)
                    .Include(dt => dt.Document);
            var total = await query.CountAsync();
            var approved = await query.CountAsync(dt => dt.Document.status == status_document.approved);
            var pending = await query.CountAsync(dt => dt.Document.status == status_document.pending);
            var deleted = await query.CountAsync(dt => dt.Document.status == status_document.deleted);
            var data = await _context.tags
                .Where(t=> t.tag_id == tagId)
                .Select(t=> new ThuVienPtit.Src.Application.Tags.DTOs.Respone.TagStatDto
                {
                    tag = new ThuVienPtit.Src.Application.Tags.DTOs.Entity.TagDto
                    {
                        tag_id = t.tag_id,
                        name = t.name,
                        description = t.description,
                        slug = t.slug,
                        created_at = t.created_at,
                    },
                    toTal = total,
                    approved = approved,
                    pending = pending,
                    deleted = deleted
                })
                .FirstOrDefaultAsync();
            return data;
        }
        public async Task<(int pageNumber, int totalPage, int pageSize, int totalItem, List<ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentFilterDto>)> GetDocumentByTagFilterAsync(int tagId, int pageNumber, int? status)
        {
            int pageSize = 10;
            var query = _context.document_tags
                .AsNoTracking()
                .Where(dt => dt.tag_id == tagId)
                .Select(dt => dt.Document);

            if (status.HasValue)
                query = query.Where(d =>d.status == (status_document)status.Value);

            var totalItem = await query.CountAsync();
            var totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

            var documents = await query
                .OrderByDescending(d => d.created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new ThuVienPtit.Src.Application.Tags.DTOs.Respone.GetDocByTag.documentFilterDto
                {
                    document_id = d.document_id,
                    slug = d.slug,
                    created_at = d.created_at,
                    avt_document = d.avt_document,
                    title = d.title,
                    description = d.description,
                    status = d.status
                })
                .ToListAsync();

            return (pageNumber, totalPage, pageSize, totalItem, documents);
        }
    }
}

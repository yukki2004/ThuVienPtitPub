using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRespone;
using ThuVienPtit.Src.Application.Users.DTOs.Entities;
using ThuVienPtit.Src.Application.Users.Interface;
using ThuVienPtit.Src.Domain.Entities;
using ThuVienPtit.Src.Domain.Enum;
using ThuVienPtit.Src.Infrastructure.Data;

namespace ThuVienPtit.Src.Infrastructure.Users.Respository
{
    public class UserRespository : IUserRespository
    {
        public readonly AppDataContext _context;
        public readonly string baseUrl;
        public UserRespository(AppDataContext context, IConfiguration configuration)
        {
            _context = context;
            baseUrl = configuration["FileStorage:BaseUrl"] ?? "";
        }
        public async Task RegisterUser(Domain.Entities.users user)
        {
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<users?> GetUserByEmail(string email)
        {
            return await _context.users.AsNoTracking().FirstOrDefaultAsync(u => u.email == email);
        }
        public async Task<users?> GetUserByGoogleId(string googleid)
        {
            return await _context.users.AsNoTracking().FirstOrDefaultAsync(u => u.google_id == googleid);
        }
        public async Task Update(users user)
        {
            _context.users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<users?> GetUserByUserId(Guid userid)
        {
            return await _context.users.AsNoTracking().FirstOrDefaultAsync(u => u.user_id == userid);
        }
        public async Task<users?> GetUserByEmailOrUsernameAsync(string login)
        {
            return await _context.users.FirstOrDefaultAsync(u => u.email == login || u.username == login);
        }
        public async Task<(int totalItems, int pageNumber, int pageSize, int totalPage, List<Application.Users.DTOs.Entities.PubAndPenDocUserDTO.DocumentDto>)> GetUserInfoByIdPublishAsync(Guid userId, int pageNumber, CancellationToken cancellationToken)
        {
            var query = _context.documents
                .AsNoTracking()
                .Where(d => d.user_id == userId && d.status == status_document.approved);
            var totalItems = await query.CountAsync(cancellationToken);
            var document = await _context.documents
                .Where(d => d.user_id == userId && d.status == status_document.approved)
                .OrderByDescending(d => d.updated_at)
                .Select(d => new Application.Users.DTOs.Entities.PubAndPenDocUserDTO.DocumentDto
                {
                    document_id = d.document_id,
                    title = d.title,
                    avt_document = Path.Combine(baseUrl,d.avt_document).Replace("\\","/"),
                    slug = d.slug,
                    created_at = d.created_at,
                    description = d.description,
                    course = new Application.Users.DTOs.Entities.PubAndPenDocUserDTO.CoursesDto
                    {
                        course_id = d.course_id,
                        name = d.course.name,
                        slug = d.course.slug,
                        semester = new Application.Users.DTOs.Entities.PubAndPenDocUserDTO.SemesterDto
                        {
                            semester_id = d.course.semester_id,
                            name = d.course.semester.name,
                        }
                    },
                    tags = d.document_tags.Select(dt => new Application.Users.DTOs.Entities.PubAndPenDocUserDTO.TagDto
                    {
                        tag_id = dt.tag_id,
                        name = dt.Tag.name,
                        slug = dt.Tag.slug,
                    }).ToList()
                })
                .AsNoTracking()
                .Skip((pageNumber - 1) * 5)
                .Take(5)
                .ToListAsync(cancellationToken);
            var totalPage = (int)Math.Ceiling((double)totalItems / 5);
            var pageSize = 5;
            if (document == null || document.Count == 0)
            {
                return (totalItems, pageNumber, 0, totalPage, new List<Application.Users.DTOs.Entities.PubAndPenDocUserDTO.DocumentDto>());
            }
            return (totalItems, pageNumber, pageSize, totalPage, document);

        }
        public async Task<(int totalItems, int pageNumber, int pageSize, int totalPage, List<Application.Users.DTOs.Entities.PubAndPenDocUserDTO.DocumentDto>)> GetUserInfoByIdPendingAsync(Guid userId, int pageNumber,CancellationToken cancellationToken)
        {
            var query = _context.documents
                .AsNoTracking()
                .Where(d => d.user_id == userId && d.status == status_document.pending);
            var totalItems = await query.CountAsync(cancellationToken);
            var document = await _context.documents
                .Where(d => d.user_id == userId && d.status == status_document.pending)
                .OrderByDescending(d => d.updated_at)
                .Select(d => new Application.Users.DTOs.Entities.PubAndPenDocUserDTO.DocumentDto
                {
                    document_id = d.document_id,
                    title = d.title,
                    avt_document = Path.Combine(baseUrl, d.avt_document).Replace("\\", "/"),
                    slug = d.slug,
                    created_at = d.created_at,
                    description = d.description,
                    course = new Application.Users.DTOs.Entities.PubAndPenDocUserDTO.CoursesDto
                    {
                        course_id = d.course_id,
                        name = d.course.name,
                        slug = d.course.slug,
                        semester = new Application.Users.DTOs.Entities.PubAndPenDocUserDTO.SemesterDto
                        {
                            semester_id = d.course.semester_id,
                            name = d.course.semester.name,
                        }
                    },
                    tags = d.document_tags.Select(dt => new Application.Users.DTOs.Entities.PubAndPenDocUserDTO.TagDto
                    {
                        tag_id = dt.tag_id,
                        name = dt.Tag.name,
                        slug = dt.Tag.slug,
                    }).ToList()
                })
                .AsNoTracking()
                .Skip((pageNumber - 1) * 5)
                .Take(5)
                .ToListAsync(cancellationToken);
            var totalPage = (int)Math.Ceiling((double)totalItems / 5);
            var pageSize = 5;
            if (document == null || document.Count == 0)
            {
                return (totalItems, pageNumber, 0, totalPage, new List<Application.Users.DTOs.Entities.PubAndPenDocUserDTO.DocumentDto>());
            }
            return (totalItems, pageNumber, pageSize, totalPage, document);

        }
        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.user_id == userId);
            if (user != null)
            {
                _context.users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<(int totalItems, int pageNumber, int pageSize, int totalPage, List<users>)> GetAllUserAsync(int pageNumber)
        {
            var totalItems = await _context.users.CountAsync();
            var userList  = await _context.users
                .OrderByDescending(u=>u.created_at)
                .Skip((pageNumber - 1)*10)
                .Take(10)
                .AsNoTracking()
                .ToListAsync();
            var totalPage = (int)Math.Ceiling((double)totalItems / 10);
            var pageSize = 10;
            return (totalItems, pageNumber, pageSize, totalPage, userList);



        }
    }
}

using Microsoft.EntityFrameworkCore;
using ThuVienPtit.Src.Application.Courses.DTOs.Entity;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse;
using ThuVienPtit.Src.Application.Courses.Interface;
using ThuVienPtit.Src.Domain.Entities;
using ThuVienPtit.Src.Domain.Enum;
using ThuVienPtit.Src.Infrastructure.Data;

namespace ThuVienPtit.Src.Infrastructure.Courses.Respository
{
    public class CourseRespository : ICourseRespository
    {
        private AppDataContext _context;
        public CourseRespository(AppDataContext context)
        {
            _context = context;
        }
        public async Task AddCourseAsync(Domain.Entities.courses course)
        {
            await _context.courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }
        public async Task<CategoryCourse?> GetCategoryCourseAsync(Guid courseId, CancellationToken cancellationToken)
        {
            var categoryCourse = await _context.courses
                .Where(c => c.course_id == courseId)
                .Select(c => new CategoryCourse
                {
                    Course = new CourseDto
                    {
                        course_id = c.course_id,
                        name = c.name,
                        description = c.description,
                        credits = c.credits,
                        slug = c.slug,
                        created_at = c.created_at,
                        updated_at = c.updated_at,
                        semester_id = c.semester_id,
                    },
                    Category = new ThuVienPtit.Src.Application.Courses.DTOs.Entity.CategoryDto
                    {
                        category_id = c.category_id,
                        name = c.category.name,
                        description = c.category.description,
                        created_at = c.category.created_at,
                    }
                })
                .FirstOrDefaultAsync(cancellationToken);
            return categoryCourse;
        }
        public async Task DeleteCourseAsync(Guid courseId)
        {
            var course = await _context.courses.FindAsync(courseId);
            if (course != null)
            {
                _context.courses.Remove(course);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<courses?> GetCoursesById(Guid courseId)
        {
            return await _context.courses.AsNoTracking().FirstOrDefaultAsync(c=> c.course_id == courseId);
        }
        public async Task UpdateCourseAsync(Domain.Entities.courses course)
        {
            _context.courses.Update(course);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetAllCourseRespone>> GetAllCoursesAsync()
        {
            var respone = await _context.courses
                .AsNoTracking()
                .Select(c=> new ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetAllCourseRespone
                {
                    course_id = c.course_id,
                    name = c.name,
                    slug = c.slug,
                })
                .ToListAsync();
            return respone;
        }
        public async Task<(int pageNumber, int pageSize, int totalPage, int toTalItem, Application.Courses.DTOs.Respone.GetDocByCourseCategory.courseDto courseDto, List<Application.Courses.DTOs.Respone.GetDocByCourseCategory.documentDto>)> GetCategoryDocumentCourseAsync(int pageNumber, string category, string slug, CancellationToken cancellationToken)
        {
            var cou = await _context.courses.FirstOrDefaultAsync(c=> c.slug == slug, cancellationToken);
            if(cou == null)
            {
                throw new Exception("không tìm thấy khóa học trong database");
            }
            var course = new Application.Courses.DTOs.Respone.GetDocByCourseCategory.courseDto
            {
                course_id = cou.course_id,
                slug = cou.slug,
                name = cou.name,
                description = cou.description,
                credits = cou.credits,
            };
            var docs =  _context.documents
                .AsNoTracking()
                .Where(d => d.course_id == cou.course_id && d.category == category && d.status == Domain.Enum.status_document.approved);
            var totalItems = await docs.CountAsync(cancellationToken);
            var documents = await _context.documents
                .Where(d => d.status == Domain.Enum.status_document.approved && d.category == category && d.course_id == cou.course_id)
                .AsNoTracking()
                .OrderByDescending(d => d.created_at)
                .Select(d=> new Application.Courses.DTOs.Respone.GetDocByCourseCategory.documentDto
                {
                    document_id = d.document_id,
                    avt_document = d.avt_document,
                    title = d.title,
                    description = d.description,
                    slug = d.slug,
                    created_at = d.created_at,
                    tags = d.document_tags.Select(dt=> new Application.Courses.DTOs.Respone.GetDocByCourseCategory.TagDocDto
                    {
                        tag_id = dt.tag_id,
                        name = dt.Tag.name,
                        slug=dt.Tag.slug,
                    }).ToList()
                })
                .Skip((pageNumber - 1) * 10)
                .Take(10)
                .ToListAsync(cancellationToken);
            var pageSize = 10;
            var pageNumbers = pageNumber;
            var tolTalPage = (int)Math.Ceiling((double)totalItems / 10);
            return (pageNumbers, pageSize, tolTalPage, totalItems,course, documents);

        }
        public async Task<List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.CategoryListRespone>> GetAllCategoryDocumentAsync()
        {
            var respone = await _context.course_categories
                .Select(ct => new ThuVienPtit.Src.Application.Courses.DTOs.Respone.CategoryListRespone
                {
                    category_id = ct.category_id,
                    name = ct.name,
                })
                .ToListAsync();
            return respone;
        }
        public async Task<(int pageNumber, int pageSize, int toTalPage, int tolTalItem, List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage.courseDto>)> GetCourseListPageAsync(int pageNumber, Guid? semesterId)
        {
            var query = _context.courses
                .AsNoTracking()
                .AsQueryable();
            if (semesterId.HasValue && semesterId != Guid.Empty)
                query = query.Where(c => c.semester_id == semesterId);
            var totalItems = await query.CountAsync();
            var data = await query
                .OrderByDescending(c => c.created_at)
                .Skip((pageNumber - 1) * 10)
                .Take(10)
                .Select(c => new ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage.courseDto
                {
                    course_id = c.course_id,
                    name = c.name,
                    description = c.description,
                    slug = c.slug,
                    created_at = c.created_at,
                    credits = c.credits,
                    category_Course = new ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage.category_course
                    {
                        category_id = c.category_id,
                        name = c.category.name,
                    },
                    semesterDto = new ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage.semesterDto
                    {
                        semester_id = c.semester_id,
                        name = c.semester.name,
                    }
                })
                .ToListAsync();
            int TotalPages = (int)Math.Ceiling(totalItems / (double)10);
            int pageSize = 10;
            return (pageNumber, pageSize, TotalPages, totalItems, data);
        }
        public async Task<DocumentStatisticSummaryDto> GetCourseDocumentStatisticsAsync(Guid courseid)
        {
            var query = _context.documents
                .AsNoTracking()
                .Where(d => d.course_id == courseid);

            var documents = await query.ToListAsync();

            var total = documents.Count;
            var approved = documents.Count(d => d.status == status_document.approved);
            var pending = documents.Count(d => d.status == status_document.pending);
            var deleted = documents.Count(d => d.status == status_document.deleted);

            var byCategory = documents
                .GroupBy(d => d.category)
                .ToDictionary(g => g.Key, g => g.Count());

            return new DocumentStatisticSummaryDto
            {
                Total = total,
                Approved = approved,
                Pending = pending,
                Deleted = deleted,
                ByCategory = byCategory
            };
        }
        public async Task<ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse.CourseDetailDto?> GetCourseBySlugAsync(string slug)
        {
            var course = await _context.courses
                .AsNoTracking()
                .Include(c => c.category)
                .Include(c => c.semester)
                .FirstOrDefaultAsync(c => c.slug == slug);

            if (course == null)
                return null;

            return new ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse.CourseDetailDto
            {
                CourseId = course.course_id,
                Name = course.name,
                Description = course.description,
                Credits = course.credits,
                Slug = course.slug,
                CreatedAt = course.created_at,
                Category = new Application.Courses.DTOs.Respone.InfoCourse.CategoryDto
                {
                    CategoryId = course.category.category_id,
                    Name = course.category.name
                },
                Semester = new Application.Courses.DTOs.Respone.InfoCourse.SemesterDto
                {
                    SemesterId = course.semester.semester_id,
                    Name = course.semester.name,
                    Year = course.semester.year
                }
            };
        }
        public async Task<(List<documents> Items, int TotalItem)> GetDocumentsByCourseAsync(Guid courseId, int pageNumber, int pageSize = 10, string? category = null, int? status = null)
        {
            var query = _context.documents
                .AsNoTracking()
                .Where(d => d.course_id == courseId);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(d => d.category == category);

            if (status.HasValue)
            {
                var statusEnum = (status_document)status.Value;
                query = query.Where(d => d.status == statusEnum);
            }

            int totalItem = await query.CountAsync();

            var items = await query
                .OrderByDescending(d => d.created_at)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItem);
        }
    }
}

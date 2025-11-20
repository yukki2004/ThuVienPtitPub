using Microsoft.EntityFrameworkCore;
using ThuVienPtit.Src.Application.Semesters.DTOs.Entities;
using ThuVienPtit.Src.Application.Semesters.Interface;
using ThuVienPtit.Src.Domain.Entities;
using ThuVienPtit.Src.Infrastructure.Data;

namespace ThuVienPtit.Src.Infrastructure.Semesters.Respository
{
    public class SemesterRespository : ISemesterRespository
    {
        private readonly AppDataContext _context;
        public SemesterRespository(AppDataContext context)
        {
            _context = context;
        }
        public async Task AddSemesterAsync(Domain.Entities.semesters semester)
        {
            await _context.semesters.AddAsync(semester);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteSemesterAsync(Guid semesterId)
        {
            var semester = _context.semesters.FirstOrDefault(s => s.semester_id == semesterId);
            if (semester != null)
            {
                _context.semesters.Remove(semester);
            }
            await _context.SaveChangesAsync();
        }
        public async Task UpdateSemesterAsync(Domain.Entities.semesters semester)
        {
            _context.semesters.Update(semester);
            await _context.SaveChangesAsync();
        }
        public async Task<semesters?> GetSemesterByName(string name)
        {
            var semester =  await _context.semesters.AsNoTracking().FirstOrDefaultAsync(s => s.name == name);
            return semester;
        }
        public async Task<semesters?> GetSemesterById(Guid semesterId)
        {
            var semester = await _context.semesters.AsNoTracking().FirstOrDefaultAsync(s => s.semester_id == semesterId);
            return semester;
        }
        public async Task<List<semesters>> GetAllSemesterAsync()
        {
            var semesters = await _context.semesters.AsNoTracking().ToListAsync();
            return semesters;
        }
        public async Task<List<Application.Semesters.DTOs.Respone.GetFullCouresSemester.semester_course_category>> GetSemester_course_category()
        {
            var result = await _context.semesters
                .Where(s => s.courses.Any())
                .AsNoTracking()
                .Select(s => new Application.Semesters.DTOs.Respone.GetFullCouresSemester.semester_course_category
                {
                    semester = new Application.Semesters.DTOs.Respone.GetFullCouresSemester.SemesterDto
                    {
                        semester_id = s.semester_id,
                        name = s.name,
                        courses = 
                            s.courses.Select(c => new Application.Semesters.DTOs.Respone.GetFullCouresSemester.CourseDto
                            {
                                course_id = c.course_id,
                                name = c.name,
                                credits = c.credits,
                                slug = c.slug,
                                category_Course = new Application.Semesters.DTOs.Respone.GetFullCouresSemester.CategoryCoursesDto
                                {
                                    category_id = c.category.category_id,
                                    name = c.category.name,
                                    description = c.category.description
                                }
                            }).ToList()
                    }
                })
                .ToListAsync();
            return result;
        }
       public async Task<Application.Semesters.DTOs.Respone.GetFullCouresSemester.SemesterDto?> GetCourseBySemesterAsync(Guid semesterId)
        {
            return await _context.semesters
                .Select(s => new Application.Semesters.DTOs.Respone.GetFullCouresSemester.SemesterDto
                {
                    semester_id = s.semester_id,
                    name = s.name,
                    courses = s.courses.Select(c=> new Application.Semesters.DTOs.Respone.GetFullCouresSemester.CourseDto
                    {
                        course_id = c.course_id,
                        name = c.name
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.semester_id == semesterId);
        }
    }
}

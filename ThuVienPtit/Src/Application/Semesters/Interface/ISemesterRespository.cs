using ThuVienPtit.Src.Application.Semesters.DTOs.Entities;
using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Semesters.Interface
{
    public interface ISemesterRespository
    {
        Task<List<semesters>> GetAllSemesterAsync();
        Task<Application.Semesters.DTOs.Respone.GetFullCouresSemester.SemesterDto?> GetCourseBySemesterAsync(Guid semesterId);
        Task AddSemesterAsync(Domain.Entities.semesters semester);
        Task DeleteSemesterAsync(Guid semesterId);
        Task UpdateSemesterAsync(Domain.Entities.semesters semester);
        Task<semesters?> GetSemesterByName(string name);
        Task<semesters?> GetSemesterById(Guid semesterId);
        Task<List<Application.Semesters.DTOs.Respone.GetFullCouresSemester.semester_course_category>> GetSemester_course_category();

    }
}

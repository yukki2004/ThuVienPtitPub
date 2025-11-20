using ThuVienPtit.Src.Application.Courses.DTOs.Entity;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse;
using ThuVienPtit.Src.Domain.Entities;
using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Application.Courses.Interface
{
    public interface ICourseRespository
    {
        Task AddCourseAsync(Domain.Entities.courses course);
        Task UpdateCourseAsync(Domain.Entities.courses course);
        Task<CategoryCourse?> GetCategoryCourseAsync(Guid courseId, CancellationToken cancellationToken);
        Task DeleteCourseAsync(Guid courseId);
        Task<courses?> GetCoursesById(Guid coursesId);
        Task<List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetAllCourseRespone>> GetAllCoursesAsync();
        Task<(int pageNumber, int pageSize, int totalPage, int toTalItem, Application.Courses.DTOs.Respone.GetDocByCourseCategory.courseDto courseDto, List<Application.Courses.DTOs.Respone.GetDocByCourseCategory.documentDto>)> GetCategoryDocumentCourseAsync(int pageNumber, string category, string slug, CancellationToken cancellationToken);
        Task<List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.CategoryListRespone>> GetAllCategoryDocumentAsync();
        Task<(int pageNumber, int pageSize, int toTalPage, int tolTalItem, List<ThuVienPtit.Src.Application.Courses.DTOs.Respone.GetCoursePage.courseDto>)> GetCourseListPageAsync(int pageNumber, Guid? semesterId);
        Task<DocumentStatisticSummaryDto> GetCourseDocumentStatisticsAsync(Guid courseid);
        Task<ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse.CourseDetailDto?> GetCourseBySlugAsync(string slug);
        Task<(List<documents> Items, int TotalItem)> GetDocumentsByCourseAsync(
       Guid courseId,
       int pageNumber,
       int pageSize = 10,
       string? category = null,
       int? status = null);
    }
}

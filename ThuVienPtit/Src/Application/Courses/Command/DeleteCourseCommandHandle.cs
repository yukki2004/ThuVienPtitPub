using MediatR;
using ThuVienPtit.Src.Application.Courses.Interface;
using ThuVienPtit.Src.Application.Interface;
using ThuVienPtit.Src.Domain.Entities;

namespace ThuVienPtit.Src.Application.Courses.Command
{
    public class DeleteCourseCommandHandle : IRequestHandler<DeleteCourseCommand, bool>
    {
        private readonly ICourseRespository _courseRespository;
        private readonly IFileStorageService _fileStorageService;
        private readonly Semesters.Interface.ISemesterRespository _semesterRespository;
        private readonly ICacheService cacheService;
        public DeleteCourseCommandHandle(ICourseRespository courseRespository, IFileStorageService fileStorageService, Semesters.Interface.ISemesterRespository semesterRespository, ICacheService cacheService)
        {
            _courseRespository = courseRespository;
            _fileStorageService = fileStorageService;
            _semesterRespository = semesterRespository;
            this.cacheService = cacheService;
        }
        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var categoryCourse = await _courseRespository.GetCoursesById(request.CourseId);
            if (categoryCourse == null)
            {
                throw new Exception("Không tìm thấy khóa học trong database");
            }
            var semesterName = await _semesterRespository.GetSemesterById(categoryCourse.semester_id);
            if(semesterName == null)
            {
                throw new Exception("không tìm thấy thư mục của môn cần xóa");
            }
            await _fileStorageService.DeleteCourseFolder(categoryCourse.name, semesterName.name);
            await _courseRespository.DeleteCourseAsync(request.CourseId);
            // xử lý cache
            var cacheKey = "thuvienptit:course:all";
            await cacheService.RemoveAsync(cacheKey);
            var cacheKeySemesterCourse = "semester_course_category_all";
            await cacheService.RemoveAsync(cacheKeySemesterCourse);
            var cacheKeySemester = $"Semester:{categoryCourse.semester_id}";
            await cacheService.RemoveAsync(cacheKeySemester);
            return true;
        }

    }
}

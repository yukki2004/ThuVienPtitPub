using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;
using ThuVienPtit.Src.Application.Courses.Interface;
using ThuVienPtit.Src.Application.Interface;
using ThuVienPtit.Src.Application.Semesters.Interface;

namespace ThuVienPtit.Src.Application.Courses.Command
{
    public class CreateCourseCommandHandle : IRequestHandler<CreateCourseCommand, CreateCourseResponeDto>
    {
        private readonly ICourseRespository courseRespository;
        private readonly Courses.Interface.IFileStorageService fileStorageService;
        private readonly ISemesterRespository semesterRespository;
        private readonly ICacheService cacheService;
        private readonly ISlugHelper helper;
        public CreateCourseCommandHandle(ICourseRespository courseRespository, Courses.Interface.IFileStorageService fileStorageService, ISemesterRespository semesterRespository, ICacheService cacheService, ISlugHelper helper)
        {
            this.courseRespository = courseRespository;
            this.fileStorageService = fileStorageService;
            this.semesterRespository = semesterRespository;
            this.cacheService = cacheService;
            this.helper = helper;
        }
        public async Task<CreateCourseResponeDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
           
            var course = new Domain.Entities.courses
            {
                course_id = Guid.NewGuid(),
                name = request.name,
                description = request.description,
                credits = request.credits,
                semester_id = request.semester_id,
                slug = helper.GenerateSlug(request.name),
                category_id = request.category_id,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };
            await courseRespository.AddCourseAsync(course);
            var semester = await semesterRespository.GetSemesterById(request.semester_id);
            if(semester == null)
            {
                throw new Exception("không tôn tại học kỳ");
            }
            await fileStorageService.CreateCourseFolder(request.name, semester.name);
            var existingCourse = await courseRespository.GetCategoryCourseAsync(course.course_id, cancellationToken);
            // xử lý cache
            var cacheKeyCourseAll = "thuvienptit:course:all";
            await cacheService.RemoveAsync(cacheKeyCourseAll);
            var cacheKeySemesterCourse = "semester_course_category_all";
            await cacheService.RemoveAsync(cacheKeySemesterCourse);
            var cacheKeySemester = $"Semester:{request.semester_id}";
            await cacheService.RemoveAsync(cacheKeySemester);
            return new CreateCourseResponeDto
            {
                course_id = course.course_id,
                name = course.name,
                description = course.description,
                credits = course.credits,
                semester_id = course.semester_id,
                slug = course.slug,
                created_at = course.created_at,
                updated_at = course.updated_at,
                CategoryCourse = existingCourse.Category
            };
        }


    }
}

using MediatR;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone;
using ThuVienPtit.Src.Application.Courses.Interface;
using ThuVienPtit.Src.Application.Semesters.Interface;
using IFileStorageService = ThuVienPtit.Src.Application.Courses.Interface.IFileStorageService;

namespace ThuVienPtit.Src.Application.Courses.Command
{
    public class UpdateCourseCommandHandle : IRequestHandler<UpdateCourseCommand, UpdateCourseCommandDto>
    {
        private readonly ICourseRespository courseRespository;
        public UpdateCourseCommandHandle(ICourseRespository courseRespository)
        {
            this.courseRespository = courseRespository;
        }
        public async Task<UpdateCourseCommandDto> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var newCourse = await courseRespository.GetCoursesById(request.course_id);
            if (newCourse == null)
            {
                throw new Exception("Khóa học không tồn tại");
            }
            newCourse.description = request.description;
            newCourse.credits = request.credits;
            newCourse.category_id = request.category_id;
            newCourse.updated_at = DateTime.UtcNow;
            await courseRespository.UpdateCourseAsync(newCourse);

            var categoryCourse = await courseRespository.GetCategoryCourseAsync(newCourse.course_id, cancellationToken);
            return new UpdateCourseCommandDto
            {
                course_id = newCourse.course_id,
                name = newCourse.name,
                description = newCourse.description,
                credits = newCourse.credits,
                semester_id = newCourse.semester_id,
                slug = newCourse.slug,
                created_at = newCourse.created_at,
                updated_at = newCourse.updated_at,
                CategoryCourse = categoryCourse.Category
            };
        }
    }
}

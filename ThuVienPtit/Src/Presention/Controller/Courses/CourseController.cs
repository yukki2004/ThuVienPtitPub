using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThuVienPtit.Src.Application.Courses.Command;
using ThuVienPtit.Src.Application.Courses.DTOs.Respone.InfoCourse;
using ThuVienPtit.Src.Application.Courses.Queries;
using ThuVienPtit.Src.Domain.Enum;

namespace ThuVienPtit.Src.Presention.Controller.Courses
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IMediator mediator;
        public CourseController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("create-course")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCourse([FromBody] Application.Courses.Command.CreateCourseCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("Delete-course/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var command = new DeleteCourseCommand
            {
                CourseId = id
            };
            var result = await mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "Xóa khóa học thành công" });
            }
            else
            {
                return BadRequest(new { message = "Xóa khóa học thất bại" });
            }
        }
        [HttpPut("update-course/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] Application.Courses.DTOs.Request.UpdateCourseRequestDto command)
        {
            var comman = new Application.Courses.Command.UpdateCourseCommand
            {
                course_id = id,
                description = command.description,
                credits = command.credits,
                category_id = command.category_id,
            };
            var result = await mediator.Send(comman);
            return Ok(result);
        }
        [HttpGet("get-courses")]
        public async Task<IActionResult> GetCourses()
        {
            var query = new Application.Courses.Queries.GetAllCourseQueries();
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("{courseSlug}/documents")]
        public async Task<IActionResult> GetDocumentByCourse([FromRoute] string courseSlug, [FromQuery] string category, [FromQuery] int pageNumber)
        {
            var queries = new GetDocumentByCategoryCourseQueries
            {
                slug = courseSlug,
                category = category,
                pageNumber = pageNumber
            };
            var result = await mediator.Send(queries);
            return Ok(result);
        }
        [HttpGet("get-category-course")]
        public async Task<IActionResult> GetCategoryCourse()
        {
            var queries = new GetAllCategoryDocumentQueries();
            var result = await mediator.Send(queries);
            return Ok(result);
        }
        [HttpGet("get-all-course-admin")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetCourseByAdmin([FromQuery] Guid? semesterId, [FromQuery] int pageNumber = 1)
        {
            var queries = new GetCourseListPageQueries
            {
                pageNumber = pageNumber,
                semester_id = semesterId
            };
            var result = await mediator.Send(queries);
            return Ok(result);
        }
        [HttpGet("SatisticsCourse/{courseid}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetSatisticsCourse([FromRoute] Guid courseid)
        {
            var queries = new GetSatisticsCourseQueries
            {
                courseid = courseid
            };
            var result = await mediator.Send(queries);
            return Ok(result);
        }
        [HttpGet("InfoCourse/{slug}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetInfoCourse([FromRoute] string slug)
        {
            var queries = new GetInfoCourseQueries
            {
                slug = slug
            };
            var result = await mediator.Send(queries);
            return Ok(result);
        }
        [HttpGet("get-document-admin/{courseId}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetDocumentsByCourse(
       [FromRoute] Guid courseId,
       [FromQuery] int pageNumber = 1,
       [FromQuery] string? category = null,
       [FromQuery] int? status = null)
        {
            var query = new GetAllDocumentByCourseQueries
            {
                course_id = courseId,
                pageNumber = pageNumber,
                category = category,
                status = status
            };

            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}

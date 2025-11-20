using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ThuVienPtit.Src.Presention.Controller.Semesters
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterController : ControllerBase
    {
        private readonly IMediator mediator;
        public SemesterController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("create-semester")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateSemester([FromBody] Application.Semesters.Command.CreateSemesterCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        [HttpPut("update-semester/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateSemester(Guid id, [FromBody] Application.Semesters.DTOs.Request.UpdateSemesterRequestDto command)
        {
            var comman = new Application.Semesters.Command.UpdateSemesterCommand
            {
                semester_id = id,
                name = command.name,
                year = command.year
            };
            var result = await mediator.Send(comman);
            return Ok(result);
        }
        [HttpDelete("delete-semester/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteSemester(Guid id)
        {
            var command = new Application.Semesters.Command.DeleteSemesterCommand
            {
                semester_id = id
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("get-all-semesters")]
        public async Task<IActionResult> GetAllSemesters()
        {
            var query = new Application.Semesters.Queries.GetAllSemesterQueries();
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("get-semester-course-categories")]
        public async Task<IActionResult> GetAllSemesterCourseCategory()
        {
            var query = new Application.Semesters.Queries.GetAllSemesterCourseCategoryQueries();
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("{id}/courses")]
        public async Task<IActionResult> GetCoursesBySemester(Guid id)
        {
            var query = new Application.Semesters.Queries.GetCourseBySemesterQueries
            {
                SemesterId = id
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThuVienPtit.Src.Application.Tags.Queries;

namespace ThuVienPtit.Src.Presention.Controller.Tags
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly IMediator mediator;
        public TagController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("create-tag")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateTag([FromBody] Application.Tags.Command.CreateTagCommand command)
        {
            var result = await mediator.Send(command);
            return  Ok(result);
        }
        [HttpDelete("delete-tag/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var command = new Application.Tags.Command.DeleteTagCommand
            {
                tag_id = id
            };
            var result = await mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "Xóa tag thành công" });
            }
            else
            {
                return BadRequest(new { message = "Xóa tag thất bại" });
            }
        }
        [HttpGet("get-all-tags")]
        public async Task<IActionResult> GetAllTags()
        {
            var query = new Application.Tags.Queries.GetAllTagQueries();
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("get-tag-by-slug/{slug}")]
        public async Task<IActionResult> GetTagBySlug(string slug, int pageNumber = 1)
        {
            var query = new Application.Tags.Queries.GetDocumentByTagQueries
            {
                slug = slug,
                pageNumber = pageNumber
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("get-all-list-tag-admin")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetAllListTagAdmin([FromQuery] int pageNumber = 1)
        {
            var query = new ThuVienPtit.Src.Application.Tags.Queries.GetAllListTagQueries
            {
                pageNumber = pageNumber
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("get-stat-tag-admin/{tagId}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetStatTagAdmin([FromRoute] int tagId)
        {
            var query = new ThuVienPtit.Src.Application.Tags.Queries.GetTagStatQueries
            {
                tagId = tagId
            };
            var resuption = await mediator.Send(query);
            return Ok(resuption);
        }
        [HttpGet("get-document-filter-admin/{tagId}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetDocumentFilterAdmin([FromRoute] int tagId, [FromQuery] int? status, [FromQuery] int pageNumber = 1)
        {
            var query = new GetTagDocumentPageFitlerQueries
            {
                tagId = tagId,
                pageNumber = pageNumber,
                status = status
            };
            var res = await mediator.Send(query);
            return Ok(res);
        }

    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThuVienPtit.Src.Application.DashBoard.DTOs;
using ThuVienPtit.Src.Application.DashBoard.Queries;

namespace ThuVienPtit.Src.Presention.Controller.DashBoard
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashBoardController : ControllerBase
    {
        private readonly IMediator mediator;

        public DashBoardController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("statistics")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await mediator.Send(new GetAdminStatisticsQueries());
            return Ok(result);
        }
    }
}

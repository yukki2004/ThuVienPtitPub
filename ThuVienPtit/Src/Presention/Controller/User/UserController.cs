using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using ThuVienPtit.Src.Application.Users.Queries;
using ThuVienPtit.Src.Application.Users.DTOs.DtoRequest;
using System.Security.Claims;
using ThuVienPtit.Src.Application.Users.Command;

namespace ThuVienPtit.Src.Presention.Controller.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Application.Users.Command.RegisterUserCommand command)
        {
            await mediator.Send(command);
            return Ok(new { message = "User registered successfully" });
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] Application.Users.Command.RefreshTokenCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Application.Users.Queries.UserLoginQueries command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut([FromBody] LogoutUserQueries logoutUserQueries)
        {
            var result = await mediator.Send(logoutUserQueries);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userClaims = HttpContext?.User;
            if (userClaims?.Identity == null || !userClaims.Identity.IsAuthenticated)
                return Unauthorized(new { Errors = new[] { "Chưa xác thực" } });

            var idClaim = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim))
                return BadRequest(new { Errors = new[] { "Không lấy được userId từ token" } });

            var currentUserId = Guid.Parse(idClaim);
            var command = new ChangePasswordCommand
            {
                userid = currentUserId,
                oldPassword = changePasswordDto.oldPassword,
                newPassword = changePasswordDto.newPassword,
                confirmPassword = changePasswordDto.confirmPassword,
            };
            var result = await mediator.Send(command);
            if (result == "Sửa mật khẩu thành công")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserRequestDto updateUserRequestDto)
        {
            var userClaims = HttpContext?.User;
            if (userClaims?.Identity == null || !userClaims.Identity.IsAuthenticated)
                return Unauthorized(new { Errors = new[] { "Chưa xác thực" } });

            var idClaim = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim))
                return BadRequest(new { Errors = new[] { "Không lấy được userId từ token" } });
            var currentUserId = Guid.Parse(idClaim);
            var command = new UpdateUserCommand
            {
                userid = currentUserId,
                userName = updateUserRequestDto.userName,
                name = updateUserRequestDto.name,
                major = updateUserRequestDto.major,
                stream = updateUserRequestDto.img?.OpenReadStream(),
                imageName = updateUserRequestDto.img?.FileName,
            };
            var result = await mediator.Send(command);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Lỗi không tìm được user");
            }

        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            await mediator.Send(command);
            return Ok(new { message = "Mã xác nhận đã được gửi đến email của bạn nếu email tồn tại trong hệ thống." });
        }
        [HttpPost("VerifyPassword")]
        public async Task<IActionResult> VerifyPassword([FromBody] VerifyPasswordCommand command)
        {
            var result = await mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "Mã xác nhận hợp lệ." });
            }
            else
            {
                return BadRequest(new { message = "Mã xác nhận không hợp lệ." });
            }
        }
        [HttpPatch("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetTokenCommand command)
        {
            var result = await mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "Đặt lại mật khẩu thành công, vui lòng đăng nhập lại." });
            }
            else
            {
                return BadRequest(new { message = "Đặt lại mật khẩu thất bại. Vui lòng kiểm tra mã xác nhận và thử lại." });
            }
        }
        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginQueries queries)
        {
            var result = await mediator.Send(queries);
            return Ok(result);
        }
        // user lấy ra thông tin của chính mình
        [HttpGet("GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var userClaims = HttpContext?.User;
            if (userClaims?.Identity == null || !userClaims.Identity.IsAuthenticated)
                return Unauthorized(new { Errors = new[] { "Chưa xác thực" } });
            var idClaim = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim))
                return BadRequest(new { Errors = new[] { "Không lấy được userId từ token" } });
            var currentUserId = Guid.Parse(idClaim);
            var query = new GetUserInfoQueries
            {
                user_id = currentUserId
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        // lấy ra thông tin user bởi admin
        [HttpGet("get-user-by-admin/{userid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUserByAdmin([FromRoute] Guid userid)
        {
            var query = new GetUserInfoQueries
            {
                user_id = userid
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        // lấy ra tất cả doc đã publish của user
        [HttpGet("GetPubDoc")]
        [Authorize]
        public async Task<IActionResult> GetPubDoc([FromQuery] int pageNumber = 1)
        {
            var userClaims = HttpContext?.User;
            if (userClaims?.Identity == null || !userClaims.Identity.IsAuthenticated)
                return Unauthorized(new { Errors = new[] { "Chưa xác thực" } });
            var idClaim = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim))
                return BadRequest(new { Errors = new[] { "Không lấy được userId từ token" } });
            var currentUserId = Guid.Parse(idClaim);
            var query = new GetDocumentPublishUserQueries
            {
                user_id = currentUserId,
                pageNumber = pageNumber
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        // lấy ra tất cả doc đang pending của user
        [HttpGet("GetPenDoc")]
        [Authorize]
        public async Task<IActionResult> GetPenDoc([FromQuery] int pageNumber = 1)
        {
            var userClaims = HttpContext?.User;
            if (userClaims?.Identity == null || !userClaims.Identity.IsAuthenticated)
                return Unauthorized(new { Errors = new[] { "Chưa xác thực" } });
            var idClaim = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim))
                return BadRequest(new { Errors = new[] { "Không lấy được userId từ token" } });
            var currentUserId = Guid.Parse(idClaim);
            var query = new GetDocumentPendingUserQueries
            {
                user_id = currentUserId,
                pageNumber = pageNumber
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        // admin có thẻ lấy doc đã publish của user bất kỳ
        [HttpGet("GetPubDocByAdmin/{userid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetPubDocByAdmin([FromRoute] Guid userid,CancellationToken cancellationToken, [FromQuery] int pageNumber = 1)
        {
            var query = new GetDocumentPublishUserQueries
            {
                user_id = userid,
                pageNumber = pageNumber
            };
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        // admin có thẻ lấy doc đang pending của user bất kỳ
        [HttpGet("GetPenDocByAdmin/{userid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetPenDocByAdmin([FromRoute] Guid userid, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1)
        {
            var query = new GetDocumentPendingUserQueries
            {
                user_id = userid,
                pageNumber = pageNumber
            };
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        [HttpDelete("DeleteUser/{userid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userid)
        {
            var command = new DeleteUserCommand
            {
                user_id = userid
            };
            var result = await mediator.Send(command);
            if (result)
            {
                return Ok(new { message = "User deleted successfully" });
            }
            else
            {
                return BadRequest(new { message = "Delete user failed" });
            }
        }
        [HttpGet("Get-All-User")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUser([FromQuery] int pageNumber = 1)
        {
            var queries = new GetListUserQueries
            {
                pageNumber = pageNumber
            };
            var result = await mediator.Send(queries);
            return Ok(result);
        }
    }
}

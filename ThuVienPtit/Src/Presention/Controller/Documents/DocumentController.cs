using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThuVienPtit.Src.Application.Documents.Command;
using ThuVienPtit.Src.Application.Documents.Queries;

namespace ThuVienPtit.Src.Presention.Controller.Documents
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IMediator mediator;
        public DocumentController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("add-document")]
        [Authorize]
        public async Task<IActionResult> AddDocument([FromForm] Application.Documents.DTOs.Request.CreateDocumentRequestDto dto)
        {
            var userClaims = HttpContext?.User;
            if (userClaims?.Identity == null || !userClaims.Identity.IsAuthenticated)
                return Unauthorized(new { Errors = new[] { "Chưa xác thực" } });
            var userRole = userClaims.FindFirstValue(ClaimTypes.Role);
            if(userRole == null)
                return BadRequest(new { Errors = new[] { "Không lấy được role từ token" } });
            var idClaim = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim))
                return BadRequest(new { Errors = new[] { "Không lấy được userId từ token" } });
            var currentUserId = Guid.Parse(idClaim);
            var command = new Application.Documents.Command.CreateDocumentCommand
            {
                title = dto.title,
                description = dto.description,
                category = dto.category,
                course_id = dto.course_id,
                semester_id = dto.semester_id,
                avtDocument = dto.avtDocument,
                fileDocument = dto.fileDocument,
                user_id = currentUserId,
                userRole = userRole,
                tag_ids = dto.tag_ids
            };
            var result = await mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("DeleteDocument/{documentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteDocument([FromRoute] Guid documentId)
        {
            var userClaims = HttpContext?.User;
            if (userClaims?.Identity == null || !userClaims.Identity.IsAuthenticated)
                return Unauthorized(new { Errors = new[] { "Chưa xác thực" } });
            var idClaim = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idClaim))
                return BadRequest(new { Errors = new[] { "Không lấy được userId từ token" } });
            var currentUserId = Guid.Parse(idClaim);
            var command = new Application.Documents.Command.DeleteDocumentCommand
            {
                document_id = documentId,
                user_id = currentUserId,
                user_role = userClaims.FindFirstValue(ClaimTypes.Role) ?? ""
            };
            var result = await mediator.Send(command);
            if(result)
            {
                return Ok(new { Message = "Xóa tài liệu thành công" });
            }
            else
            {
                return BadRequest(new {Massage = "không có quyền xóa tài liệu"});
            }

        }
        [HttpPatch("RejectDocument/{documentId}")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> RejectDocument([FromRoute] Guid documentId)
        {
            var command = new Application.Documents.Command.RejectDocumentCommand
            {
                document_id = documentId,
            };
            var result = await mediator.Send(command);
            if (result)
            {
                return Ok(new { Message = "Từ chối tài liệu thành công" });
            }
            else
            {
                return BadRequest(new { Massage = "Từ chối tài liệu không thành công" });
            }
        }
        [HttpPatch("ApproveDocument/{documentId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ApproveDocument(Guid documentId)
        {
            var command = new ApproveDocumentCommand
            {
                document_id = documentId,
            };
            var result = await mediator.Send(command);
            if (!result)
            {
                return BadRequest(new { message = "duyệt thất bại" });
            }
            else
            {
                return Ok(new { massage = "duyệt bài viết thành công" });
            }
        }
        [HttpDelete("ClearDocument/{documentId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ClearDocument(Guid documentId)
        {
            var command = new ClearDocumentCommand
            {
                document_id = documentId,
            };
            var result = await mediator.Send(command);
            if (!result)
            {
                return BadRequest(new { message = "xóa document thất bại" });
            }
            else
            {
                return Ok(new { massage = "xóa document thành công" });
            }
        }
        [HttpPatch("RestoreDocument/{documentId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RestoreDocument(Guid documentId)
        {
            var command = new RestoreDocumentCommand
            {
                document_id = documentId,
            };
            var result = await mediator.Send(command);
            if (!result)
            {
                return BadRequest(new { message = "khôi phục thất bại" });
            }
            else
            {
                return Ok(new { massage = "khôi phục thành công" });
            }
        }
        [HttpGet("GetDocumentBySlug/{slug}")]
        public async Task<IActionResult> GetDocumentBySlug([FromRoute] string slug)
        {
            var userClaims = HttpContext?.User;
            var userRole = userClaims?.FindFirstValue(ClaimTypes.Role) ?? "guest";
            var idClaim = userClaims?.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            var currentUserId = (idClaim != null) ? Guid.Parse(idClaim) : (Guid?)null;
            var query = new Application.Documents.Queries.GetDocumentByIdQueries
            {
                slug = slug,
                userRole = userRole,
                user_id = currentUserId
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("{id}/related")]
        public async Task<IActionResult> GetRelatedDocuments([FromRoute] Guid id)
        {
            var query = new Application.Documents.Queries.GetRelatedDocumentQueries
            {
               document_id  = id
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("AdminPendingDocuments")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetPendingDocumentsAdmin([FromQuery] int pageNumber = 1)
        {
            var query = new Application.Documents.Queries.GetPendingDocumentQueries
            {
                pageNumber = pageNumber
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("AdminDeleteDocuments")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetDeleteDocumentsAdmin([FromQuery] int pageNumber = 1)
        {
            var query = new Application.Documents.Queries.GetDeleteDocumentQueries
            {
                pageNumber = pageNumber
            };
            var result = await mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("AdminPublishDocuments")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetPublishDocumentAdmin([FromQuery] int pageNumber = 1)
        {
            var queries = new GetPublishDocumentQueries
            {
                pageNumber = pageNumber,
            };
            var result = await mediator.Send(queries);
            return Ok(result);
        }

    }
}

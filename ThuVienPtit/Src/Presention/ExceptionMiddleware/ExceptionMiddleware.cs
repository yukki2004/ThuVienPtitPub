using System.Net;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
namespace ThuVienPtit.Src.Presention.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            var response = new
            {
                success = false,
                error = exception.GetType().Name,
                message = exception.Message,
                details = (string?)null
            };

            switch (exception)
            {
                case ValidationException validationEx:
                    status = HttpStatusCode.BadRequest;
                    var errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList();
                    await WriteJsonResponse(context, status, new
                    {
                        success = false,
                        message = "Dữ liệu không hợp lệ",
                        errors
                    });
                    return;

                case KeyNotFoundException:
                    status = HttpStatusCode.NotFound;
                    await WriteJsonResponse(context, status, new
                    {
                        success = false,
                        message = "Không tìm thấy tài nguyên"
                    });
                    return;

                case UnauthorizedAccessException:
                    status = HttpStatusCode.Unauthorized;
                    await WriteJsonResponse(context, status, new
                    {
                        success = false,
                        message = "Bạn không có quyền truy cập"
                    });
                    return;

                case DbUpdateException:
                    status = HttpStatusCode.BadRequest;
                    await WriteJsonResponse(context, status, new
                    {
                        success = false,
                        message = "Lỗi khi cập nhật cơ sở dữ liệu"
                    });
                    return;
            }

            // Mặc định: lỗi 500
            await WriteJsonResponse(context, status, response);
        }

        private static async Task WriteJsonResponse(HttpContext context, HttpStatusCode status, object response)
        {
            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
using FluentValidation;
using System.Net;

namespace LinhLong.Api.Middleware
{
    public class ExceptionMappingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMappingMiddleware> _logger;

        public ExceptionMappingMiddleware(RequestDelegate next, ILogger<ExceptionMappingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (ValidationException vex)
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await ctx.Response.WriteAsJsonAsync(new
                {
                    title = "Validation failed",
                    status = 400,
                    errors = vex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                });
            }
            catch (UnauthorizedAccessException uex)
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await ctx.Response.WriteAsJsonAsync(new { title = "Unauthorized", status = 401, detail = uex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await ctx.Response.WriteAsJsonAsync(new { title = "Server error", status = 500 });
            }
        }
    }

    public static class ExceptionMappingExtensions
    {
        public static IApplicationBuilder UseExceptionMapping(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionMappingMiddleware>();
    }
}

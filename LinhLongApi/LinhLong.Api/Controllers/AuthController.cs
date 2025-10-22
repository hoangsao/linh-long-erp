using LinhLong.Application.Auth.Commands.Login;
using LinhLong.Application.Auth.Commands.Logout;
using LinhLong.Application.Auth.Commands.Refresh;
using LinhLong.Application.Auth.DTOs;
using LinhLong.Application.Auth.Queries.GetMe;
using LinhLong.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LinhLong.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly JwtOptions _jwt;
        private readonly bool _crossSite;

        public AuthController(IMediator mediator, IOptions<JwtOptions> jwt, IConfiguration cfg)
        {
            _mediator = mediator;
            _jwt = jwt.Value;

            var fe = cfg["FrontendOrigin"] ?? "";
            _crossSite = IsCrossSite(fe, HttpContext?.Request?.Host.Host);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new LoginCommand(dto), ct);

            SetAccessCookie(Response, result.AccessToken, DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes), _crossSite);
            SetRefreshCookie(Response, result.RefreshToken, result.RefreshExpiresUtc, _crossSite);

            return Ok(new { roles = result.Roles });
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            if (!Request.Cookies.TryGetValue("refresh_token", out var refresh))
                return Unauthorized();

            var result = await _mediator.Send(new RefreshCommand(refresh), ct);

            SetAccessCookie(Response, result.AccessToken, DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes), _crossSite);
            SetRefreshCookie(Response, result.RefreshToken, result.RefreshExpiresUtc, _crossSite);

            return Ok(new { roles = result.Roles });
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Me(CancellationToken ct)
        {
            var me = await _mediator.Send(new GetMeQuery(), ct);
            return Ok(me);
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            if (Request.Cookies.TryGetValue("refresh_token", out var refresh))
                await _mediator.Send(new LogoutCommand(refresh), ct);

            ClearAuthCookies(Response);
            return NoContent();
        }

        private static void SetAccessCookie(HttpResponse res, string token, DateTime expiresUtc, bool crossSite)
        {
            res.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = crossSite ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = expiresUtc,
                Path = "/"
            });
        }

        private static void SetRefreshCookie(HttpResponse res, string token, DateTime expiresUtc, bool crossSite)
        {
            res.Cookies.Append("refresh_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = crossSite ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = expiresUtc,
                Path = "/"
            });
        }

        private static void ClearAuthCookies(HttpResponse res)
        {
            res.Cookies.Delete("access_token", new CookieOptions { Path = "/" });
            res.Cookies.Delete("refresh_token", new CookieOptions { Path = "/" });
        }

        private static bool IsCrossSite(string feOrigin, string? apiHost)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(feOrigin) || string.IsNullOrWhiteSpace(apiHost))
                    return false;

                var fe = new Uri(feOrigin);
                var feSite = GetETldPlusOne(fe.Host);
                var apiSite = GetETldPlusOne(apiHost);
                return !string.Equals(feSite, apiSite, StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }

            static string GetETldPlusOne(string host)
            {
                var parts = host.Split('.', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) return host;
                return $"{parts[^2]}.{parts[^1]}";
            }
        }
    }
}

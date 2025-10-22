using LinhLong.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LinhLong.Infrastructure.Auth
{
    public class CurrentUser : ICurrentUser
    {
        public Guid? UserId { get; }
        public string? UserName { get; }
        public IReadOnlyCollection<string> Roles { get; }

        public CurrentUser(IHttpContextAccessor accessor)
        {
            var user = accessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                UserId = Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
                UserName = user.FindFirstValue(ClaimTypes.Name);
                Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            }
            else
            {
                Roles = Array.Empty<string>();
            }
        }
    }
}

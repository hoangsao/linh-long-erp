using Microsoft.AspNetCore.Identity;

namespace LinhLong.Infrastructure.Auth
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}

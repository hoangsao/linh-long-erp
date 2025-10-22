using LinhLong.Domain.Common;

namespace LinhLong.Infrastructure.Auth
{
    /// <summary>
    /// Stores persistent refresh tokens for JWT authentication.
    /// </summary>
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; } = default!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime? RevokedAtUtc { get; private set; }
        public string? ReplacedByToken { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => RevokedAtUtc == null && !IsExpired;

        protected RefreshToken() { }

        public RefreshToken(Guid userId, string token, DateTime expiresAt)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
        }

        public void Revoke(string? replacedBy = null)
        {
            RevokedAtUtc = DateTime.UtcNow;
            ReplacedByToken = replacedBy;
        }
    }
}

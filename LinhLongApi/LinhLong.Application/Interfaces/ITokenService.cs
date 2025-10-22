namespace LinhLong.Application.Interfaces
{
    public interface ITokenService
    {
        Task<(string AccessToken, string RefreshToken, DateTime RefreshExpiresUtc)> IssueAsync(Guid userId, string userName, IEnumerable<string> roles, CancellationToken ct = default);
        Task<(string AccessToken, string NewRefreshToken, DateTime RefreshExpiresUtc, IEnumerable<string> Roles)?> RotateAsync(string refreshToken, CancellationToken ct = default);
        Task RevokeAsync(string refreshToken, CancellationToken ct = default);
    }
}

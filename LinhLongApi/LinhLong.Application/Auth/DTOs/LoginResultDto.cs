namespace LinhLong.Application.Auth.DTOs
{
    public sealed record LoginResultDto(string AccessToken, string RefreshToken, DateTime RefreshExpiresUtc, IEnumerable<string> Roles);
}

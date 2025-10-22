namespace LinhLong.Application.Auth.DTOs
{
    public sealed record UserInfoDto(Guid Id, string UserName, IEnumerable<string> Roles);
}

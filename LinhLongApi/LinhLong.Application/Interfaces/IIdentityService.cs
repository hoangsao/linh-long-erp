namespace LinhLong.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, bool IsLockedOut)> CheckPasswordAsync(string username, string password, bool lockoutOnFailure, CancellationToken ct = default);
        Task<(Guid Id, string UserName, IEnumerable<string> Roles)?> GetUserWithRolesAsync(string username, CancellationToken ct = default);
    }
}

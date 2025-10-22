namespace LinhLong.Application.Interfaces
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }
        string? UserName { get; }
        IReadOnlyCollection<string> Roles { get; }
    }
}

using MediatR;

namespace LinhLong.Application.Auth.Commands.Logout
{
    public sealed record LogoutCommand(string RefreshToken) : IRequest<Unit>;
}

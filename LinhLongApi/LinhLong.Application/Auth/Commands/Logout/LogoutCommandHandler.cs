using LinhLong.Application.Interfaces;
using MediatR;

namespace LinhLong.Application.Auth.Commands.Logout
{
    public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
    {
        private readonly ITokenService _tokens;

        public LogoutCommandHandler(ITokenService tokens) => _tokens = tokens;

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken ct)
        {
            await _tokens.RevokeAsync(request.RefreshToken, ct);
            return Unit.Value;
        }
    }
}

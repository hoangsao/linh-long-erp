using LinhLong.Application.Auth.DTOs;
using LinhLong.Application.Interfaces;
using MediatR;

namespace LinhLong.Application.Auth.Commands.Refresh
{

    public sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, LoginResultDto>
    {
        private readonly ITokenService _tokens;

        public RefreshCommandHandler(ITokenService tokens) => _tokens = tokens;

        public async Task<LoginResultDto> Handle(RefreshCommand request, CancellationToken ct)
        {
            var rotated = await _tokens.RotateAsync(request.RefreshToken, ct);
            if (rotated is null) throw new UnauthorizedAccessException("Invalid refresh token.");

            return new LoginResultDto(rotated.Value.AccessToken, rotated.Value.NewRefreshToken, rotated.Value.RefreshExpiresUtc, rotated.Value.Roles);
        }
    }
}

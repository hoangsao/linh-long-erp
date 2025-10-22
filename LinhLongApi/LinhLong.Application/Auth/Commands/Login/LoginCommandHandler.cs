using LinhLong.Application.Auth.DTOs;
using LinhLong.Application.Interfaces;
using MediatR;

namespace LinhLong.Application.Auth.Commands.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
    {
        private readonly IIdentityService _identity;
        private readonly ITokenService _tokens;

        public LoginCommandHandler(IIdentityService identity, ITokenService tokens)
        {
            _identity = identity;
            _tokens = tokens;
        }

        public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken ct)
        {
            var (ok, locked) = await _identity.CheckPasswordAsync(
                request.Request.Username, request.Request.Password, lockoutOnFailure: true, ct);

            if (!ok)
            {
                if (locked) throw new UnauthorizedAccessException("Account locked out.");
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var user = await _identity.GetUserWithRolesAsync(request.Request.Username, ct)
                       ?? throw new UnauthorizedAccessException("User not found.");

            var issued = await _tokens.IssueAsync(user.Id, user.UserName, user.Roles, ct);

            return new LoginResultDto(issued.AccessToken, issued.RefreshToken, issued.RefreshExpiresUtc, user.Roles);
        }
    }
}

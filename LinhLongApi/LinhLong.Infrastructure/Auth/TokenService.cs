using LinhLong.Application.Interfaces;
using LinhLong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace LinhLong.Infrastructure.Auth
{
    public class TokenService : ITokenService
    {
        private readonly AppDbContext _dbContext;
        private readonly JwtOptions _jwtOptions;

        public TokenService(AppDbContext dbContext, IOptions<JwtOptions> jwtOptions)
        {
            _dbContext = dbContext;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<(string AccessToken, string RefreshToken, DateTime RefreshExpiresUtc)> IssueAsync(
            Guid userId, string userName, IEnumerable<string> roles, CancellationToken ct = default)
        {
            var key = new SymmetricSecurityKey(Convert.FromBase64String(_jwtOptions.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Name, userName)
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes),
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            var tokenStr = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refresh = new RefreshToken(userId, tokenStr, DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays));

            _dbContext.RefreshTokens.Add(refresh);
            await _dbContext.SaveChangesAsync(ct);

            return (accessToken, refresh.Token, refresh.ExpiresAt);
        }

        public async Task<(string AccessToken, string NewRefreshToken, DateTime RefreshExpiresUtc, IEnumerable<string> Roles)?>
            RotateAsync(string refreshToken, CancellationToken ct = default)
        {
            var existing = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken, ct);

            if (existing == null || !existing.IsActive)
                return null;

            existing.Revoke();

            var user = await _dbContext.Users.FindAsync(new object[] { existing.UserId }, ct);
            if (user == null) return null;

            var roles = await _dbContext.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_dbContext.Roles, ur => ur.RoleId, r => r.Id, (_, r) => r.Name!)
                .ToListAsync(ct);

            var issued = await IssueAsync(user.Id, user.UserName!, roles, ct);

            existing.Revoke(issued.RefreshToken);

            await _dbContext.SaveChangesAsync(ct);

            return (issued.AccessToken, issued.RefreshToken, issued.RefreshExpiresUtc, roles);
        }

        public async Task RevokeAsync(string refreshToken, CancellationToken ct = default)
        {
            var entity = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken, ct);
            if (entity is null) return;
            entity.Revoke();
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}

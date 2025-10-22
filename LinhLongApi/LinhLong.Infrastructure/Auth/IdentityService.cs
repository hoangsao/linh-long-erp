using LinhLong.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LinhLong.Infrastructure.Auth
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sm)
        {
            _userManager = um;
            _signInManager = sm;
        }

        public async Task<(bool Succeeded, bool IsLockedOut)> CheckPasswordAsync(string username, string password, bool lockoutOnFailure, CancellationToken ct = default)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null) return (false, false);
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            return (result.Succeeded, result.IsLockedOut);
        }

        public async Task<(Guid Id, string UserName, IEnumerable<string> Roles)?> GetUserWithRolesAsync(string username, CancellationToken ct = default)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            return (user.Id, user.UserName!, roles);
        }
    }
}

using LinhLong.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LinhLong.Infrastructure.Data
{
    public static class AppDbInitializer
    {
        public static async Task SeedAsync(IServiceProvider provider)
        {
            var db = provider.GetRequiredService<AppDbContext>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

            await db.Database.MigrateAsync();

            if (await userManager.Users.AnyAsync())
                return;

            string[] roles = new[] { "Admin", "Editor", "Viewer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@linhlong.test",
                EmailConfirmed = true
            };
            var adminResult = await userManager.CreateAsync(admin, "Admin@123456!");
            if (adminResult.Succeeded)
            {
                await userManager.AddToRolesAsync(admin, new[] { "Admin", "Editor" });
            }

            var viewer = new ApplicationUser
            {
                UserName = "viewer",
                Email = "viewer@linhlong.test",
                EmailConfirmed = true
            };
            var viewerResult = await userManager.CreateAsync(viewer, "Viewer@123456!");
            if (viewerResult.Succeeded)
            {
                await userManager.AddToRoleAsync(viewer, "Viewer");
            }
        }
    }
}

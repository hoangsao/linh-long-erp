using LinhLong.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinhLong.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RefreshToken>(cfg =>
            {
                cfg.ToTable("RefreshTokens");
                cfg.HasKey(x => x.Id);
                cfg.HasIndex(x => x.Token).IsUnique();
                cfg.Property(x => x.Token).HasMaxLength(256);
                cfg.HasOne<ApplicationUser>()
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

using g.identity.dataAccess.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace g.identity.dataAccess;

public class IdentityAppContext : IdentityDbContext<
    AppUser,
    AppRole,
    string,
    IdentityUserClaim<string>,
    AppUserRole,
    IdentityUserLogin<string>,
    IdentityRoleClaim<string>,
    IdentityUserToken<string>>
{
    public DbSet<Application> Applications { get; set; }
    public DbSet<OtpCode> OtpCodes { get; set; }
    public DbSet<SecondFactor> SecondFactors { get; set; }

    public IdentityAppContext(DbContextOptions<IdentityAppContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
using Microsoft.AspNetCore.Identity;

namespace g.identity.dataAccess.Domain;

public class AppUserRole : IdentityUserRole<string>
{
    public AppUser User { get; set; }
    public AppRole Role { get; set; }
}
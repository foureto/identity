using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace g.identity.dataAccess.Domain;

public class AppUser : IdentityUser<string>
{
    [MaxLength(255)] public string Nickname { get; set; }
    [MaxLength(128)] public string AppId { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    
    public Application App { get; set; }
    public List<AppUserRole> UserRoles { get; set; } = new();
}
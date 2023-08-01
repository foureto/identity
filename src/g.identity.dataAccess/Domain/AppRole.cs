using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace g.identity.dataAccess.Domain;

public class AppRole : IdentityRole<string>
{
    [MaxLength(128)] public string AppId { get; set; }
    
    public Application App { get; set; }
    public List<AppUserRole> UserRoles { get; set; } = new();

    public string CreatedById { get; set; }
    public AppUser CreatedBy { get; set; }
    
    public string UpdatedById { get; set; }
    public AppUser UpdatedBy { get; set; }
}
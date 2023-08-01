using System.ComponentModel.DataAnnotations;

namespace g.identity.dataAccess.Domain;

public class Application
{
    [MaxLength(128)] public string Id { get; set; }
    [MaxLength(255)] public string Name { get; set; }
    [MaxLength(255)] public string Description { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    
    public string CreatedById { get; set; } 
    public AppUser CreatedBy { get; set; } 

    public string UpdatedById { get; set; } 
    public AppUser UpdatedBy { get; set; }
    
    public List<AppUser> Users { get; set; } = new();
    public List<AppRole> Roles { get; set; } = new();
}
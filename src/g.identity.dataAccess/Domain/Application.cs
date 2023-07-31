using System.ComponentModel.DataAnnotations;

namespace g.identity.dataAccess.Domain;

public class Application
{
    [MaxLength(128)] public string Id { get; set; }
    [MaxLength(255)] public string Name { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public List<AppUser> Users { get; set; } = new();
    public List<AppRole> Roles { get; set; } = new();
}
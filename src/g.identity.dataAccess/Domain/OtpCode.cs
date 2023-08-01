using System.ComponentModel.DataAnnotations;

namespace g.identity.dataAccess.Domain;

public class OtpCode
{
    [Key] [MaxLength(128)] public string Jti { get; set; }
    [MaxLength(128)] public string UserId { get; set; }
    [MaxLength(128)] public string AppId { get; set; }
    [MaxLength(10)] public string Code { get; set; }
    public bool Validated { get; set; }
    public bool HasSecondFactor { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Updated { get; set; } = DateTime.UtcNow;
}
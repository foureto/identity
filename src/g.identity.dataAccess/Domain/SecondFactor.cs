namespace g.identity.dataAccess.Domain;

public class SecondFactor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; }
    public string AppId { get; set; }
    public byte[] Hash { get; set; }
    public string Token { get; set; }
    public long TimeToLive { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }
}
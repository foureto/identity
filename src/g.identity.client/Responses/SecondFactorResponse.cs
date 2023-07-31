using System.Runtime.Serialization;

namespace g.identity.client.Responses;

[DataContract]
public class SecondFactorResponse
{
    [DataMember(Order = 1)] public string Hash { get; set; }
    [DataMember(Order = 2)] public string Token { get; set; }
    [DataMember(Order = 3)] public long TimeToLive { get; set; }
}
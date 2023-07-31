using System.Runtime.Serialization;

namespace g.identity.client.Requests;

[DataContract]
public class UpdateSecondFactorRequest
{
    [DataMember(Order = 1)] public string UserId { get; set; }
    [DataMember(Order = 2)] public string AppId { get; set; }
    [DataMember(Order = 3)] public bool Enable { get; set; }
}
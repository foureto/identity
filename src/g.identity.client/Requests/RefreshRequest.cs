using System.Runtime.Serialization;

namespace g.identity.client.Requests;

[DataContract]
public class RefreshRequest
{
    [DataMember(Order = 1)] public string RefreshToken { get; set; }
}
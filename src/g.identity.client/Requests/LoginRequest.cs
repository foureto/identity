using System.Runtime.Serialization;

namespace g.identity.client.Requests;

[DataContract]
public class LoginRequest
{
    [DataMember(Order = 1)] public string ApplicationId { get; set; }
    [DataMember(Order = 2)] public string Email { get; set; }
}
using System.Runtime.Serialization;

namespace g.identity.client.Requests;

[DataContract]
public class LoginConfirmRequest
{
    [DataMember(Order = 1)] public string UserId { get; set; }
    [DataMember(Order = 2)] public string ApplicationId { get; set; }
    [DataMember(Order = 3)] public string Jti { get; set; }
    [DataMember(Order = 4)] public string Code { get; set; }
}
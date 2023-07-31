using System.Runtime.Serialization;

namespace g.identity.client.Responses;

[DataContract]
public class TokenResponse
{
    [DataMember(Order = 1)] public string AccessToken { get; set; }
    [DataMember(Order = 2)] public string RefreshToken { get; set; }
}
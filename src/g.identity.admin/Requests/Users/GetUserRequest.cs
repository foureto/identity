using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Users;

[DataContract]
public class GetUserRequest
{
    [DataMember(Order = 1)] public string UserId { get; set; }
    [DataMember(Order = 2)] public string ApplicationId { get; set; }
}
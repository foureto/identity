using System.Runtime.Serialization;

namespace g.identity.admin.Responses.Users;

[DataContract]
public class UserResponse
{
    [DataMember(Order = 1)] public string UserId { get; set; }
    [DataMember(Order = 2)] public string ApplicationId { get; set; }
    [DataMember(Order = 3)] public string ClientId { get; set; }
    [DataMember(Order = 4)] public string Email { get; set; }
    [DataMember(Order = 5)] public string NickName { get; set; }
    [DataMember(Order = 6)] public string Phone { get; set; }
    [DataMember(Order = 7)] public string UpdatedBy { get; set; }
}
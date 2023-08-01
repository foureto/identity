using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Users;

[DataContract]
public class GetUsersRequest
{
    [DataMember(Order = 1)] public int Page { get; set; }
    [DataMember(Order = 2)] public int Count { get; set; }
    [DataMember(Order = 3)] public string ApplicationId { get; set; }
    [DataMember(Order = 4)] public string Email { get; set; }
    [DataMember(Order = 5)] public string Phone { get; set; }
    [DataMember(Order = 6)] public string NickName { get; set; }
    [DataMember(Order = 7)] public string RoleId { get; set; }
}
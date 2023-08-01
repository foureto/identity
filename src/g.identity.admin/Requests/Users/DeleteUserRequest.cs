using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Users;

[DataContract]
public class DeleteUserRequest
{
    [DataMember(Order = 1)] public string UserId { get; set; }
    [DataMember(Order = 2)] public string ApplicationId { get; set; }
    [DataMember(Order = 3)] public string RemovedById { get; set; }
}
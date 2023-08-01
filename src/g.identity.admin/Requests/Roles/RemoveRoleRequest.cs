using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Roles;

[DataContract]
public class RemoveRoleRequest
{
    [DataMember(Order = 1)] public string RoleId { get; set; }
    [DataMember(Order = 2)] public string RemovedById { get; set; }
}
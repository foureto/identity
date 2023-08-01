using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Roles;

[DataContract]
public class GetRoleRequest
{
    [DataMember(Order = 1)] public string RoleId { get; set; }
}
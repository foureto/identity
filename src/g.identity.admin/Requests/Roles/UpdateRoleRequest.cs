using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Roles;

[DataContract]
public class UpdateRoleRequest
{
    [DataMember(Order = 1)] public string RoleId { get; set; }
    [DataMember(Order = 2)] public string ApplicationId { get; set; }
    [DataMember(Order = 3)] public string Name { get; set; }
    [DataMember(Order = 4)] public string UpdatedById { get; set; }
}
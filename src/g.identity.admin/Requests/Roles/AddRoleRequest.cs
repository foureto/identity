using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Roles;

[DataContract]
public class AddRoleRequest
{
    [DataMember(Order = 1)] public string ApplicationId { get; set; }
    [DataMember(Order = 2)] public string Name { get; set; }
    [DataMember(Order = 3)] public string CreatedById { get; set; }
}
using System.Runtime.Serialization;

namespace g.identity.admin.Responses.Roles;

[DataContract]
public class RoleDetailsResponse
{
    [DataMember(Order = 1)] public string RoleId { get; set; }
    [DataMember(Order = 2)] public string ApplicationId { get; set; }
    [DataMember(Order = 3)] public string Name { get; set; }
    [DataMember(Order = 4)] public string CreatedBy { get; set; }
    [DataMember(Order = 5)] public string UpdatedBy { get; set; }
}
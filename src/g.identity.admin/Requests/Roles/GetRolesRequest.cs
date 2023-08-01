using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Roles;

[DataContract]
public class GetRolesRequest
{
    [DataMember(Order = 1)] public int Page { get; set; }
    [DataMember(Order = 2)] public int Count { get; set; }
    [DataMember(Order = 3)] public string ApplicationId { get; set; }
}
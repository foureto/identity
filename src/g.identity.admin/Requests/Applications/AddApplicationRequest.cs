using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Applications;

[DataContract]
public class AddApplicationRequest
{
    [DataMember(Order = 1)] public string Name { get; set; }
    [DataMember(Order = 2)] public string Description { get; set; }
    [DataMember(Order = 3)] public string CreatedById { get; set; }
}
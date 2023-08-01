using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Applications;

[DataContract]
public class UpdateApplicationRequest
{
    [DataMember(Order = 1)] public string ApplicationId { get; set; }
    [DataMember(Order = 2)] public string Name { get; set; }
    [DataMember(Order = 3)] public string Description { get; set; }
    [DataMember(Order = 4)] public string UpdatedById { get; set; }
}
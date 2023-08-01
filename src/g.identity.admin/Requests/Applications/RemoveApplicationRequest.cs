using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Applications;

[DataContract]
public class RemoveApplicationRequest
{
    [DataMember(Order = 1)] public string ApplicationId { get; set; }
    [DataMember(Order = 2)] public string RemovedById { get; set; }
}
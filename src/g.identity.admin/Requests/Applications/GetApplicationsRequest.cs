using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Applications;

[DataContract]
public class GetApplicationsRequest
{
    [DataMember(Order = 1)] public int Page { get; set; }
    [DataMember(Order = 2)] public int Count { get; set; }
}
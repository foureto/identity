using System.Runtime.Serialization;

namespace g.identity.admin.Requests.Applications;

[DataContract]
public class GetApplicationRequest
{
    [DataMember(Order = 1)] public string ApplicationId { get; set; }
}
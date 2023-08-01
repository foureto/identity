using System.Runtime.Serialization;

namespace g.identity.admin.Responses.Applications;

[DataContract]
public class ApplicationDetailsResponse
{
    [DataMember(Order = 1)] public string ApplicationId { get; set; }
    [DataMember(Order = 2)] public string Name { get; set; }
    [DataMember(Order = 3)] public string Description { get; set; }
    [DataMember(Order = 4)] public string CreatedBy { get; set; }
    [DataMember(Order = 5)] public string UpdatedBy { get; set; }
}
using System.Runtime.Serialization;

namespace GSID.FileManagement.Response
{
    [DataContract]
    internal class GetResponse
    {
        [DataMember(Name="content")]
        public string Content { get; set; }
    }
}
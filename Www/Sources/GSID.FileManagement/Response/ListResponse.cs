using System.Collections.Generic;
using System.Runtime.Serialization;
using GSID.FileManagement.DTO;

namespace GSID.FileManagement.Response
{
    [DataContract]
    internal class ListResponse
    {
        [DataMember(Name="list")]
        public List<string> List { get; private set; }

        public ListResponse()
        {
            List = new List<string>();
        }     
    }
}
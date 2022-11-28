using System.Collections.Generic;
using System.Runtime.Serialization;
using GSID.FileManagement.DTO;

namespace GSID.FileManagement.Response
{
    [DataContract]
    internal class ChangedResponse
    {
        [DataMember(Name="changed")]
        public List<FileDTO> Changed { get; private set; }

        public ChangedResponse()
        {
            Changed = new List<FileDTO>();
        }
    }
}
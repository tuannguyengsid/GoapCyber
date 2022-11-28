using System.Collections.Generic;
using System.Runtime.Serialization;
using GSID.FileManagement.DTO;

namespace GSID.FileManagement.Response
{
    [DataContract]
    internal class TreeResponse
    {
        [DataMember(Name="tree")]
        public List<DTOBase> Tree { get; private set; }

        public TreeResponse()
        {
            Tree = new List<DTOBase>();
        }     
    }
}
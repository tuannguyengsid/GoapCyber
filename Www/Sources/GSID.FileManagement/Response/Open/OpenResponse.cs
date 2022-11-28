using System.Runtime.Serialization;
using GSID.FileManagement.DTO;

namespace GSID.FileManagement.Response
{
    [DataContract]
    internal class OpenResponse : OpenResponseBase
    {
        public OpenResponse(DTOBase currentWorkingDirectory, FullPath fullPath)
            : base(currentWorkingDirectory)
        {
            Options = new Options(fullPath);
            _files.Add(currentWorkingDirectory);
        }
    }
}
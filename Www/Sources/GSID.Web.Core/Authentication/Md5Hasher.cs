using GSID.Web.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Authentication
{
    public class Md5Hasher : IHasher
    {
        public string HashString(string srcString)
        {
            return HashExtensions.GetMD5Hash(srcString);
        }
    }
}

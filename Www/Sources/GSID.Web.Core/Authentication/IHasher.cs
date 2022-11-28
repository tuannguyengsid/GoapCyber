using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Authentication
{
    public interface IHasher
    {
        string HashString(string srcString);
    }
}

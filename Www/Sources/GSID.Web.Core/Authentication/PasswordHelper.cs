using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Authentication
{
    public static class PasswordHelper
    {
        public static string GenerateHashedPassword(string userPassword, string randomCode)
        {
            var hasher = Hasher.Instance;
            var hashedPassword = hasher.HashString(string.Format("{0}{1}", userPassword, randomCode));
            return hashedPassword;
        }
    }
}

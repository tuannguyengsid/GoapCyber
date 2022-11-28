using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Authentication
{
    public static class ApplicationConfiguration
    {
        /// <summary>
        /// Gets the name of the hasher type from the web.config, identified by the key "hasher" in the "appSettings" section
        /// This is supposed to be the Fully Qualified Type Name. The default is "ChanLap.Net.Domain.Hashers.Md5Hasher"
        /// You could create your own hashers. Have a look at the sample provided
        /// </summary>
        /// <value>
        /// The name of the hasher type.
        /// </value>
        public static string HasherTypeName
        {
            get
            {
                return ConfigurationManager.AppSettings["hasher"];
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Authentication
{
    public static class Hasher
    {
        private const string DefaultHasher = "GSID.Web.Core.Authentication.Md5Hasher";

        public static IHasher Instance
        {
            get
            {
                IHasher iHasher;
                var assemblyName = Assembly.Load("GSID.Web.Core").CodeBase;
                try
                {
                    var hasher = ApplicationConfiguration.HasherTypeName;
                    iHasher = (IHasher)Activator.CreateInstanceFrom(assemblyName, hasher).Unwrap();
                }
                catch
                {
                    var instance = Activator.CreateInstanceFrom(assemblyName, DefaultHasher).Unwrap();
                    iHasher = (IHasher)instance;
                }

                return iHasher;
            }
        }
    }
}

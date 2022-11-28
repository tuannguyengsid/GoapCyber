using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public abstract class DefineService
    {
        public int ConvertInt(String paramName)
        {
            int returnParam;
            if (Int32.TryParse(paramName, out returnParam))
            {
                return returnParam;
            }
            return -1;
        }
    }
}

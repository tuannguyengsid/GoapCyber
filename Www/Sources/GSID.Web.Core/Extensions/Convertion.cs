using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Extensions
{
    public static class Convertion
    {
        public static int ConvertInt(this string value)
        {
            int val = 0;
            if (int.TryParse(value, out val))
            {
                return val;
            }
            return -1;
        }

        public static int ConvertIntPaging(this string value)
        {
            int val = 1;
            if (int.TryParse(value, out val))
            {
                return val;
            }
            return 1;
        }
    }
}

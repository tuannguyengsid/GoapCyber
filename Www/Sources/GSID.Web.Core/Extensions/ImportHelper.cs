using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Extensions
{
    public static class ImportHelper
    {
        public static bool IsValidInput<T>(this string value) where T : struct
        {
            try
            {
                if (value.HasValue())
                {
                    var convertedValue = Convert.ChangeType(value, typeof(T));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsValidInputDateTime(this string value)
        {
            try
            {
                DateTime result;
                DateTime.TryParse(value, out result);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Extensions
{
    public class StringName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string SalutationName { get; set; }

        public static StringName ParseName(string name)
        {
            StringName nm = new StringName();

            if (!String.IsNullOrEmpty(name)) {
                var sp_full = name.Split(' ');
                int comma   = sp_full.Count();

                if (comma > 0)
                {
                    nm.FirstName    = sp_full[0];
                    nm.LastName     = sp_full[comma - 1];
                    string middle   = "";
                    for (int i = 1; i < comma - 1; i++)
                        middle = string.Format("{0} {1}", middle, sp_full[i]);
                    nm.MiddleName   = middle.Trim(); 
                }
            }

            return nm;
        }
    }
}

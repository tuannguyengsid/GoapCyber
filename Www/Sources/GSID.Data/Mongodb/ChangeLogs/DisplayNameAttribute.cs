using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Data.Mongodb.ChangeLogs
{
    public class DisplayNameAttribute : Attribute
    {
        public string Name { get; set; }
    }
}

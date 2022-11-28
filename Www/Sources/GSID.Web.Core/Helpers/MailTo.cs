using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Helpers
{
    public class MailTo
    {
        public string FullName { get; set; }
        public string To { get; set; }
        public string Bcc { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}

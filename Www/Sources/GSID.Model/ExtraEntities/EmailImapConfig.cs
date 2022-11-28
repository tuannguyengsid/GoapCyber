using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.ExtraEntities
{
    public class EmailImapConfig
    {
        public EmailImapConfig()
        {
            Code = "PARAMETER_EMAIL_IMAP_CONFIG";
        }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string InCommingServer { get; set; }
        public int Port { get; set; }
    }
}

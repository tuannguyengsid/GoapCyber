using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.ExtraEntities
{
    public class EmailSTMPConfig
    {
        public EmailSTMPConfig()
        {
            Code = "PARAMETER_EMAIL_STMP_CONFIG";
        }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string STMPSever { get; set; }
        public int STMPPort { get; set; }
        public string IMAPSever { get; set; }
        public int IMAPPort { get; set; }
        public string EmailNotification { get; set; }
    }
}

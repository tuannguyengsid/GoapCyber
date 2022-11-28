using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.ExtraEntities
{
    public class BFOAPIConfig
    {
        public BFOAPIConfig()
        {
            Code = "PARAMETER_BFO_API_CONFIG";
        }
        public string Code { get; set; }
        public string Url { get; set; }
        public string AppId { get; set; }
        public string SecretKey { get; set; }
    }
}

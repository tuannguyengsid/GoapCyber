﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.ExtraEntities
{
    public class EmailFollowConfig
    {
        public EmailFollowConfig()
        {
            Code = "PARAMETER_EMAIL_FOLLOW_CONFIG";
        }
        public string Code { get; set; }
        public string Email { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.ExtraEntities
{
    public class SmsBranchCmcConfig
    {
        public enum SentType : int
        {
            NoUnicode = 1,
            Unicode = 2
        }
        public SmsBranchCmcConfig()
        {
            Code = "PARAMETER_SMS_BRANCHNAME_CMC_CONFIG";
        }

        public string Code { get; set; }
        public string SendUrl { get; set; }
        public int MessageLimitSendUrl { get; set; }
        public string SendUTFUrl { get; set; }
        public int MessageLimitSendUTFUrl { get; set; }
        public string Branchname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public SentType Type { get; set; }
        public bool IsActive { get; set; }
    }

    public class TransactionSms
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DataSms Data { get; set; }
    }

    public class DataSms
    {
        public enum SmsStatus : int
        {
            Success = 1,
            InvalidUsernameOrPassword = -2,
            InvalidPhone = -3,
            ExceedsMessageLength = -4,
            InvalidTelco = -7,
            SpamMessage = -8,
            InvalidBrandname = -9
        }
        public string Brandname { get; set; }
        public string Phonenumber { get; set; }
        public string Message { get; set; }
        public SmsStatus Status { get; set; }
        public string StatusDescription { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Model.Enums
{
    public enum UserInComingType : int
    {
        None = 0,
        Imap = 1,
        Pop3 = 2
    }

    public enum UserInComingLogonType : int
    {
        None = 0,
        SSL = 1,
        TLS = 2
    }

    public enum SendingEmailInStatus : int
    {
        None    = 0,
        Sending = 1,
        Sent    = 2,
        Delay   = 3,
        Waiting = 4,
        Error   = 5,
    }

    public enum EnterpriseImportStatus : int
    {
        Success = 0,
        Error = 1
    }


    public enum ReportViolationStatus : int
    {
        None = -1,
        Waiting = 1,
        Browsed = 2
    }



    public enum EnterpriseIsType : int
    {
        None = -1,
        Enterprise = 1,
        BusinessInHome = 2
    }
    #region Careers
    public enum CareerIsParent : int
    {
        Root = 0,
    }
    #endregion

}

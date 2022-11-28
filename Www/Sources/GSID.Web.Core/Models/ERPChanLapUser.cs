using System;
using System.Security.Principal;
using System.Web.Security;


namespace GSID.Web.Core.Models
{ 
    [Serializable]
    public class ERPChanLapUser : IIdentity
    {
        public ERPChanLapUser(){}
        public ERPChanLapUser(string name, string displayName, string userId)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.UserId = userId;
        }
        public ERPChanLapUser(string name, string displayName, string userId,string roleName)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.UserId = userId;
            this.RoleName = roleName;
        }
        public ERPChanLapUser(string name, UserInfo userInfo)
            : this(name, userInfo.DisplayName, userInfo.UserId,userInfo.RoleName)
        {
            if (userInfo == null) throw new ArgumentNullException("userInfo");
            this.UserId = userInfo.UserId;
        }

        public ERPChanLapUser(FormsAuthenticationTicket ticket)
            : this(ticket.Name, UserInfo.FromString(ticket.UserData))
        {
            if (ticket == null) throw new ArgumentNullException("ticket");
        }

        public string Name { get; private set; }

        public string AuthenticationType
        {
            get { return "ChanLapSetterForms"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string DisplayName { get; private set; }
        public string RoleName { get; private set; }
        public string UserId { get; private set; }
    }
}

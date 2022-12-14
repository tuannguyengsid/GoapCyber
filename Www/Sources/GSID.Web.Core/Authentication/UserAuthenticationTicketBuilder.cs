
using System;
using System.Web.Security;
using GSID.Model.MongodbModels;
using GSID.Web.Core.Models;


namespace GSID.Web.Core.Authentication
{
    public class UserAuthenticationTicketBuilder
    {
        public static FormsAuthenticationTicket CreateAuthenticationTicket(User user)
        {
            UserInfo userInfo = CreateUserContextFromUser(user);

            var ticket = new FormsAuthenticationTicket(
                1,
                user.Email,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                false,
                userInfo.ToString());

            return ticket;
        }

        private static UserInfo CreateUserContextFromUser(User user)
        {
            var userContext = new UserInfo
            {
                //UserId = user.Id,
                //DisplayName = user.DisplayName,
                UserIdentifier = user.Email,
                //RoleName = Enum.GetName(typeof(UserRoles), user.RoleId)
            };

            return userContext;
        }
    }
}
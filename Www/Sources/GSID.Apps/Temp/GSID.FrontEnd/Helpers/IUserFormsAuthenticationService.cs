using GSID.Model.MongodbModels;
using GSID.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GSID.FrontEnd.Helpers
{
    public interface IUserFormsAuthenticationService
    {
        void SignIn(Customer user, bool createPersistentCookie, HttpContextBase httpcontext);
        void SignOut();
    }

    public class UserFormsAuthenticationService : IUserFormsAuthenticationService
    {
        public void SignIn(Customer user, bool createPersistentCookie, HttpContextBase httpcontext)
        {
            if (user == null)
                throw new ArgumentException("Value cannot be null or empty.", "userName");

            string userData = string.Format("{0}|{1}|{2}|{3}|customer", user.Id, 
                                                                user.FullName, 
                                                                user.Email,
                                                                string.IsNullOrEmpty(user.ImageSrc) ? "/Images/user_male.png" : user.ImageSrc);

            GSIDSessionFacade.GSIDSessionUserLogon = user;//add session

            FormsAuthentication.SetAuthCookie(userData, createPersistentCookie);
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(userData, createPersistentCookie);
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, userData);
            authCookie.Value = FormsAuthentication.Encrypt(newTicket);
            httpcontext.Response.Cookies.Add(authCookie);
        }

        public void SignOut()
        {
            GSIDSessionFacade.Remove(SestionName.gsidSessionUserLogon);
            FormsAuthentication.SignOut();
        }

        public FormsAuthenticationTicket Decrypt(string encryptedTicket)
        {
            return FormsAuthentication.Decrypt(encryptedTicket);
        }
    }
}
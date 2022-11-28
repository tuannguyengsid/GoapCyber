using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace GSID.FrontEnd.Attributes
{
    public class SslRequest : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext authContext)
        {
            string SSLEnabled = ConfigurationManager.AppSettings["CheckSSLEnable"];
            bool CheckSSLEnabled = !string.IsNullOrEmpty(SSLEnabled) ? bool.Parse(SSLEnabled) : false;

            //var checkHttpRequest =  authContext.HttpContext.Request.Headers["HTTP_X_SSL_REQUEST"].Equals("1");
            var CheckLocal = authContext.RequestContext.HttpContext.Request.IsLocal;
            var CheckSecureConn = authContext.RequestContext.HttpContext.Request.IsSecureConnection;
            //Bypass check for debugging environments  
            if (CheckSSLEnabled && !CheckLocal && !CheckSecureConn)
            {
                HandleNonHttpsRequest(authContext);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.CustomExceptions
{
    public class UrlNotFoundException : Exception
    {
        public UrlNotFoundException()
        {

        }

        public UrlNotFoundException(string message, params object[] parameters)
            : base(string.Format(message, parameters))
        {

        }
    }
}
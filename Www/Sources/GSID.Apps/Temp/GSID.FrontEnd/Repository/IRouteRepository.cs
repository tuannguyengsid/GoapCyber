using GSID.FrontEnd.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.Repository
{
    public interface IRouteRepository : IDisposable
    {
        IEnumerable<RouteData> Find(string url);
    }
}
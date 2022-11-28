using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GSID.FrontEnd.Startup))]
namespace GSID.FrontEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GSID.Admin.Startup))]
namespace GSID.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

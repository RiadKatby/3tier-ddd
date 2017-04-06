using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RefactorName.Web.Startup))]

namespace RefactorName.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
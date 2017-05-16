using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RefactorName.WebApp.Startup))]

namespace RefactorName.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBanNuoc.Startup))]
namespace WebBanNuoc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

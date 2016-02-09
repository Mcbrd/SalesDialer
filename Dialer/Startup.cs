using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dialer.Startup))]
namespace Dialer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

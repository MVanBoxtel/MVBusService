using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVBusService.Startup))]
namespace MVBusService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

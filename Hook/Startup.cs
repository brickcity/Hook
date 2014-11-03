using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hook.Startup))]
namespace Hook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

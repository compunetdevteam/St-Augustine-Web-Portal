using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SwiftSkool.Startup))]
namespace SwiftSkool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

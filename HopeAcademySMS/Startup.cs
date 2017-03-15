using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StAugustine.Startup))]
namespace StAugustine
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HopeAcademySMS.Startup))]
namespace HopeAcademySMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

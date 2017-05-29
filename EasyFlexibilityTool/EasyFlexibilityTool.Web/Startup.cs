using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EasyFlexibilityTool.Web.Startup))]
namespace EasyFlexibilityTool.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

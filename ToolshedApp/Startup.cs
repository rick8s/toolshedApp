using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToolshedApp.Startup))]
namespace ToolshedApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

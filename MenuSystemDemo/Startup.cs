using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MenuSystemDemo.Startup))]
namespace MenuSystemDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

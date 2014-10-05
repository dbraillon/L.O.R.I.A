using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(loria_website.Startup))]
namespace loria_website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

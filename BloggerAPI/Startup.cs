using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BloggerAPI.Startup))]
namespace BloggerAPI
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

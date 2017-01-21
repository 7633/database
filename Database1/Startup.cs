using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Database1.Startup))]
namespace Database1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

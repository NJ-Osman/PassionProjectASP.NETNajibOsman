using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PassionProjectASP.NETNajibOsman.Startup))]
namespace PassionProjectASP.NETNajibOsman
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

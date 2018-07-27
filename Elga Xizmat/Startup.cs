using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Elga_Xizmat.Startup))]
namespace Elga_Xizmat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

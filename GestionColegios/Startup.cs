using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GestionColegios.Startup))]
namespace GestionColegios
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

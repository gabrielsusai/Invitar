using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Invitar.Startup))]
namespace Invitar
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ETutor.Startup))]
namespace ETutor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AttendenceSystemWebApp.Startup))]
namespace AttendenceSystemWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

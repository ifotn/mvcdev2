using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mvc_dev_2017.Startup))]
namespace mvc_dev_2017
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

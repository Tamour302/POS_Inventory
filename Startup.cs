using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(POS_Inventory.Startup))]
namespace POS_Inventory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EvidencijaSoftvera_IlijaDivljan.Startup))]
namespace EvidencijaSoftvera_IlijaDivljan
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;
using Hangfire;

[assembly: OwinStartupAttribute(typeof(RSSFilter.Startup))]
namespace RSSFilter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}

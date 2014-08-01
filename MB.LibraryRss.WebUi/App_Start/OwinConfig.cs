using MB.LibraryRss.WebUi.App_Start;

using Microsoft.Owin;

// This attribute self-registers this class, so it does not need to be called directly in global.asax like the other *Config classes.
// See: http://docs.hangfire.io/en/latest/users-guide/getting-started/owin-bootstrapper.html
[assembly: OwinStartup(typeof(OwinConfig))]

namespace MB.LibraryRss.WebUi.App_Start
{
  using System.Web.Mvc;

  using Hangfire;
  using Hangfire.SqlServer;

  using MB.LibraryRss.WebUi.Interfaces;

  using Owin;

  public class OwinConfig
  {
    public static void RefreshTitlesJob()
    {
      var titleService = DependencyResolver.Current.GetService<ITitleService>();
      titleService.RefreshTitles();
    }

    public void Configuration(IAppBuilder app)
    {
      return;

      // todo: enable?
      app.UseHangfire(config =>
      {
        // Basic setup required to process background jobs.
        config.UseSqlServerStorage("DataConnectionString");
        config.UseServer();        
      });

      RecurringJob.AddOrUpdate(() => RefreshTitlesJob(), Cron.Minutely);
    }    
  }
}
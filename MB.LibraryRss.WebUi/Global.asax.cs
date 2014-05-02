namespace MB.LibraryRss.WebUi
{
  using System.Web.Http;
  using System.Web.Mvc;
  using System.Web.Optimization;
  using System.Web.Routing;

  using MB.LibraryRss.WebUi.App_Start;

  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();

      AutofacConfig.Register(GlobalConfiguration.Configuration);
      EfConfig.Register(); 

      WebApiConfig.Register(GlobalConfiguration.Configuration);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
  }
}

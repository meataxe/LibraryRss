namespace MB.LibraryRss.WebUi.App_Start
{
  using System.Reflection;
  using System.Web.Http;
  using System.Web.Mvc;

  using Autofac;
  using Autofac.Integration.Mvc;
  using Autofac.Integration.WebApi;

  public class AutofacConfig
  {
    public static void Register(HttpConfiguration config)
    {
      var builder = new ContainerBuilder();
      builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
      var container = builder.Build();

      DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
      GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
    }
  }
}
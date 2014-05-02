namespace MB.LibraryRss.WebUi.Infrastructure.Autofac
{
  using System.Security.Principal;
  using System.Web;

  using global::Autofac;

  using global::Autofac.Integration.Mvc;

  using global::Autofac.Integration.WebApi;

  using MB.LibraryRss.WebUi.Infrastructure.Orm;
  using MB.LibraryRss.WebUi.Infrastructure.Orm.Repos;

  public class WebUiModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      var assembly = typeof(WebUiModule).Assembly;

      builder.RegisterControllers(assembly).PropertiesAutowired();
      builder.RegisterApiControllers(assembly).PropertiesAutowired();      

      builder.RegisterFilterProvider();
      builder.RegisterModelBinderProvider();

      builder.Register(c => HttpContext.Current.User).As<IPrincipal>().InstancePerHttpRequest();

      builder.RegisterAssemblyTypes(assembly)
             .Where(t => t.Name.EndsWith("Service"))
             .AsImplementedInterfaces();

      builder.RegisterAssemblyTypes(assembly)
             .Where(t => t.Name.EndsWith("Factory"))
             .AsImplementedInterfaces();

      builder.RegisterType<RssEntities>().AsSelf();
      builder.RegisterType<UnitOfWork>().AsSelf();
    }
  }
}

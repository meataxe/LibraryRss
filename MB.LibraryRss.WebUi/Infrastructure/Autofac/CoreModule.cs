namespace MB.LibraryRss.WebUi.Infrastructure.Autofac
{
  using global::Autofac;

  using MB.LibraryRss.WebUi.Domain;

  using QDFeedParser.Xml;

  using Module = global::Autofac.Module;

  public class CoreModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      var assembly = typeof(TitleResult).Assembly;

      builder.RegisterAssemblyTypes(assembly)
             .Where(t => t.Name.EndsWith("Service"))
             .AsImplementedInterfaces();

      builder.RegisterAssemblyTypes(assembly)
             .Where(t => t.Name.EndsWith("Factory"))
             .AsImplementedInterfaces();

      builder.RegisterType<LinqFeedXmlParser>().As<IFeedXmlParser>();
    }
  }
}

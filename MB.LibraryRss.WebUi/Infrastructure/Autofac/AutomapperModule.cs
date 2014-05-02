namespace MB.LibraryRss.WebUi.Infrastructure.Autofac
{
  using System.Collections.Generic;
  using System.Reflection;
  using System.Web.Mvc;

  using global::Autofac;

  using AutoMapper;

  using Module = global::Autofac.Module;

  public class AutomapperModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
             .AssignableTo<Profile>()
             .As<Profile>();

      // Note: This causes single instances of the typeconverters to be created,
      //       but any constructor-injected parameters may go out of scope if
      //       they aren't also .SingleInstance() (they should not be), so in the
      //       typeconverter instances, dependancies should probably be passed in
      //       to the method, or resolved at run-time.
      builder.Register(GetMappingEngine).As<IMappingEngine>().SingleInstance();
    }

    private static IMappingEngine GetMappingEngine(IComponentContext context)
    {
      Mapper.Configuration.ConstructServicesUsing(DependencyResolver.Current.GetService);

      foreach (var p in context.Resolve<IEnumerable<Profile>>())
      {
        Mapper.AddProfile(p);
      }

      Mapper.AssertConfigurationIsValid();
      return Mapper.Engine;
    }
  }
}

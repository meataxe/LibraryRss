namespace MB.LibraryRss.WebUi.App_Start
{
  using System.Data.Entity;
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Infrastructure.Orm;
  using MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces;

  public static class EfConfig
  {
    public static void Register()
    {
      Database.SetInitializer<RssEntities>(null);

      var initService = DependencyResolver.Current.GetService<IDatastoreService>();
      initService.EnsureDatabaseIsInitialised();
    }
  }
}

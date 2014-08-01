namespace MB.LibraryRss.WebUi.Infrastructure.Orm
{
  using System.Data.Entity.Infrastructure;
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces;

  public class RssEntitiesFactory : IDbContextFactory<RssEntities>
  {
    public RssEntities Create()
    {
      return new RssEntities(DependencyResolver.Current.GetService<IConnectionService>());
    }
  }
}

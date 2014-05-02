namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Repos
{
  using System.Data.Entity;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Models;

  public class ElementRepository : EditableRepositoryBase<Element>
  {    
    public ElementRepository(DbContext context)
    {
      this.Context = context;
    }
  }
}

namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Repos
{
  using System.Collections.Generic;
  using System.Data.Entity;
  using System.Linq;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces;

  public class ReadOnlyRepositoryBase<T> : IRepository<T> where T : class
  {
    protected DbContext Context { get; set; }

    public List<T> FetchAll()
    {
      return this.Context.Set<T>().ToList();
    }

    public T GetById(int id)
    {
      return this.Context.Set<T>().Find(id);
    }
  }
}

namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces
{
  using System.Collections.Generic;

  public interface IRepository<T>
  {
    List<T> FetchAll();
    
    T GetById(int id);
  }
}

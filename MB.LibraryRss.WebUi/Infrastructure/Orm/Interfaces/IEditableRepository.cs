namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces
{
  public interface IEditableRepository<in T>
  {
    void Insert(T entity);

    void Delete(T entity);

    void Update(T entity);
  }
}

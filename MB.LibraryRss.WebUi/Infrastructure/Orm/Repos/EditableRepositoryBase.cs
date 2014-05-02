namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Repos
{
  using System.Data.Entity;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces;

  public class EditableRepositoryBase<T> : ReadOnlyRepositoryBase<T>, IEditableRepository<T> where T : class
  {
    public void Insert(T entity)
    {
      this.Context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
      if (this.Context.Entry(entity).State == EntityState.Detached)
      {
        this.Context.Set<T>().Attach(entity);
      }

      this.Context.Set<T>().Remove(entity);
    }

    public void Update(T entity)
    {
      if (this.Context.Entry(entity).State == EntityState.Detached)
      {
        this.Context.Set<T>().Attach(entity);
      }

      this.Context.Entry(entity).State = EntityState.Modified;
    }
  }
}

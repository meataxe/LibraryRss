namespace MB.LibraryRss.WebUi.Infrastructure.Orm
{
  using System;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Repos;

  public class UnitOfWork : IDisposable
  {
    private readonly RssEntities context;
    
    private ElementRepository elementRepository;

    private FeedRepository feedRepository;

    private bool disposed;

    public UnitOfWork(RssEntities context)
    {
      this.context = context;
    }

    public ElementRepository ElementRepository
    {
      get
      {
        return this.elementRepository ?? (this.elementRepository = new ElementRepository(this.context));
      }
    }

    public FeedRepository FeedRepository
    {
      get
      {
        return this.feedRepository ?? (this.feedRepository = new FeedRepository(this.context));
      }
    }

    public void Save()
    {
      this.context.SaveChanges();
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    } 
       
    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          this.context.Dispose();
        }
      }

      this.disposed = true;
    }       
  }
}

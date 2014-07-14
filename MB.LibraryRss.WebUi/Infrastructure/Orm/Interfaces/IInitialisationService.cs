namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces
{
  public interface IDatastoreService
  {
    void EnsureDatabaseIsInitialised(bool forceInitialisation = false);
  
    decimal FreeSpace();
  }
}


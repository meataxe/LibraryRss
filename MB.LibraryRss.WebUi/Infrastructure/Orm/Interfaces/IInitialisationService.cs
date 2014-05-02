namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces
{
  public interface IInitialisationService
  {
    void EnsureDatabaseIsInitialised(bool forceInitialisation = false);
  }
}
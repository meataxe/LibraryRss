namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Repos
{
  using System.Data.Entity;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Models;

  public class SettingRepository : EditableRepositoryBase<Setting>
  {
    public SettingRepository(DbContext context)
    {
      this.Context = context;
    }
  }
}

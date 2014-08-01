namespace MB.LibraryRss.WebUi.Infrastructure.Data
{
  using System;
  using System.Linq;

  using MB.LibraryRss.WebUi.Infrastructure.Orm;
  using MB.LibraryRss.WebUi.Infrastructure.Orm.Models;
  using MB.LibraryRss.WebUi.Infrastructure.Orm.Repos;
  using MB.LibraryRss.WebUi.Interfaces;

  public class SettingDataService : ISettingDataService
  {
    private readonly UnitOfWork uow;

    public SettingDataService(UnitOfWork uow)
    {
      this.uow = uow;      
    }

    private SettingRepository Settings
    {
      get
      {
        return this.uow.SettingRepository;
      }
    }

    public void AddOrUpdateRefreshTaskExecutionStartDate(DateTime? startDate)
    {
      var setting = this.Settings.FetchAll().FirstOrDefault(s => s.Name == "RefreshTaskExecutionStartDate");
      var insert = false;

      if (setting == null)
      {
        insert = true;
        setting = new Setting { Name = "RefreshTaskExecutionStartDate", Class = "All" };
      }
                          
      setting.Value = !startDate.HasValue ? null : startDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

      if (insert)
      {
        this.Settings.Insert(setting);  
      }
      else
      {
        this.Settings.Update(setting);
      }

      this.uow.Save();
    }

    public DateTime? GetRefreshTaskExecutionStartDate()
    {
      var settings = this.Settings.FetchAll();

      if (!settings.Any())
      {
        return null;
      }

      var setting = settings.FirstOrDefault(s => s.Name == "RefreshTaskExecutionStartDate");
      var value = setting == null ? null : setting.Value;

      DateTime temp;
      return DateTime.TryParse(value, out temp) ? (DateTime?)temp : null;
    }
  }
}

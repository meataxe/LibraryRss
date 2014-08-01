namespace MB.LibraryRss.WebUi.Interfaces
{
  using System;

  public interface ISettingDataService
  {
    void AddOrUpdateRefreshTaskExecutionStartDate(DateTime? startDate);

    DateTime? GetRefreshTaskExecutionStartDate();
  }
}
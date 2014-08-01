namespace MB.LibraryRss.WebUi.Interfaces
{
  using System;
  using System.Collections.Generic;

  using MB.LibraryRss.WebUi.Domain;

  public interface ITitleDataService
  {
    void Save(List<TitleResult> results, DateTime lastUpdated);

    List<TitleResult> GetLatest();

    DateTime? GetDateFeedLastUpdated();

    DateTime? GetDateDataLastRefreshed();
  }
}
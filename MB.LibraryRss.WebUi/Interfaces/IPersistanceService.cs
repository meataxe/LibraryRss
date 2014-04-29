namespace MB.LibraryRss.WebUi.Interfaces
{
  using System;
  using System.Collections.Generic;

  using MB.LibraryRss.WebUi.Domain;

  public interface IPersistanceService
  {
    void Save(List<TitleResult> results, DateTime lastUpdated);

    List<TitleResult> GetLatest();

    DateTime GetLastUpdated();
  }
}
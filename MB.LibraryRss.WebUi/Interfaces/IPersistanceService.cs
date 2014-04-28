namespace MB.LibraryRss.WebUi.Interfaces
{
  using System.Collections.Generic;

  using MB.LibraryRss.WebUi.Domain;

  public interface IPersistanceService
  {
    void Save(List<TitleResult> results);

    List<TitleResult> GetLatest();
  }
}
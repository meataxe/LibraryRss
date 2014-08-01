namespace MB.LibraryRss.WebUi.Interfaces
{
  using System.Collections.Generic;

  using MB.LibraryRss.WebUi.Domain;

  public interface ITitleService
  {
    List<TitleResult> GetTitles();

    List<TitleResult> RefreshTitles(bool force = false);
  }
}
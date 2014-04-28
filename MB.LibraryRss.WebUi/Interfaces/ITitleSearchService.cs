namespace MB.LibraryRss.WebUi.Interfaces
{
  using System.Collections.Generic;

  using MB.LibraryRss.WebUi.Domain;

  public interface ITitleSearchService
  {
    List<TitleResult> GetTitlesFromUrl(string url);

    List<TitleResult> GetTitlesFromFile(string path);
  }
}
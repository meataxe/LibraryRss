namespace MB.LibraryRss.WebUi.Interfaces
{
  using System.Collections.Generic;

  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Infrastructure.Core;

  public interface ITitleService
  {
    FeedSource FeedSource { get; set; }    

    List<TitleResult> GetTitles();

    void RefreshTitles();
  }
}
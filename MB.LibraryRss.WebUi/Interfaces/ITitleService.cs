namespace MB.LibraryRss.WebUi.Interfaces
{
  using System.Collections.Generic;

  using MB.LibraryRss.WebUi.Domain;

  public interface ITitleService
  {
    FeedSource FeedSource { get; set; }    

    List<TitleResult> GetTitles();

    List<TitleResult> RefreshTitles();
  }
}
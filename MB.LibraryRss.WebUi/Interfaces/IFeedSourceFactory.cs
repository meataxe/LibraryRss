namespace MB.LibraryRss.WebUi.Interfaces
{
  using MB.LibraryRss.WebUi.Domain;
  
  public interface IFeedSourceFactory
  {
    FeedSource GetFeedSource();
  }
}
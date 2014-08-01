namespace MB.LibraryRss.WebUi.Interfaces
{
  using MB.LibraryRss.WebUi.Domain;

  using QDFeedParser;

  public interface ITitleFeedFactory
  {
    IFeed DefaultFeed();

    IFeed CreateFeed(FeedSource feedSource);

    IFeed CreateFeed(string feedxml);
  }
}
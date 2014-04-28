namespace MB.LibraryRss.WebUi.Interfaces
{
  using QDFeedParser;

  public interface ICustomFeedFactory
  {
    IFeed CreateFeed(string feedxml);
  }
}
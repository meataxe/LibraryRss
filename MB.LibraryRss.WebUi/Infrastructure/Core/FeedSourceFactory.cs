namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Interfaces;

  public class FeedSourceFactory : IFeedSourceFactory
  {
    private static readonly FeedSource FeedSource = new FeedSource
      {
        Source = @"https://ent.kotui.org.nz/client/rss/hitlist/pn/qu=newbks-pn&dt=list&st=PD",
        SourceType = SearchSource.Url
      }; 

    public FeedSource GetFeedSource()
    {
      return FeedSource;
    }
  }
}
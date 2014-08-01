namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Interfaces;

  public class FeedSourceFactory : IFeedSourceFactory
  {
    public FeedSourceFactory()
    {
      this.FeedSource = new FeedSource
      {
        Source = @"https://ent.kotui.org.nz/client/rss/hitlist/pn/qu=newbks-pn&dt=list&st=PD",
        SourceType = SearchSource.Url
      };
    }

    public FeedSourceFactory(FeedSource feedSource)
    {
      this.FeedSource = feedSource;
    }

    public FeedSource FeedSource { get; private set; }
  }
}
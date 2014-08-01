namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Repos
{
  using System;
  using System.Data.Entity;
  using System.Linq;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Models;

  public class FeedRepository : EditableRepositoryBase<Feed>
  {
    public FeedRepository(DbContext context)
    {
      this.Context = context;
    }

    public Feed GetLatestFeed()
    {
      var feeds = this.FetchAll();

      return feeds.FirstOrDefault(f => f.FeedLastUpdated == feeds.Max(f1 => f1.FeedLastUpdated));
    }

    public DateTime? GetMaxFeedLastUpdated()
    {
      var feeds = this.FetchAll();

      return feeds == null || !feeds.Any() ? (DateTime?)null : feeds.Max(f => f.FeedLastUpdated);
    }

    public DateTime? GetMaxInserted()
    {
      var feeds = this.FetchAll();

      return feeds == null || !feeds.Any() ? (DateTime?)null : feeds.Max(f => f.Inserted);
    }
  }
}

namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using System.IO;
  using System.Linq;
  using System.Xml;

  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Interfaces;

  using QDFeedParser;
  using QDFeedParser.Xml;

  public class TitleFeedFactory : ITitleFeedFactory
  {
    private readonly IFeedXmlParser parser;
    private readonly IDownloadService downloadService;
    private readonly IFeedSourceFactory feedSourceFactory;

    public TitleFeedFactory(IFeedXmlParser parser, IDownloadService downloadService, IFeedSourceFactory feedSourceFactory)
    {
      this.parser = parser;
      this.downloadService = downloadService;
      this.feedSourceFactory = feedSourceFactory;
    }

    public IFeed DefaultFeed()
    {
      return this.CreateFeed(this.feedSourceFactory.FeedSource);
    }

    public IFeed CreateFeed(FeedSource feedSource)
    {
      var feedXml = feedSource.SourceType == SearchSource.File
          ? File.ReadAllLines(feedSource.Source).Aggregate(string.Empty, (current, next) => current + "\r\n" + next).Trim()
          : this.downloadService.Download(feedSource.Source);

      return this.CreateFeed(feedXml);
    }

    public IFeed CreateFeed(string feedxml)
    {
      var feedtype = this.CheckFeedType(feedxml);
      return this.CreateFeed(feedtype, feedxml);
    }

    private FeedType CheckFeedType(string feedxml)
    {
      try
      {
        return this.parser.CheckFeedType(feedxml);
      }
      catch (XmlException ex)
      {
        throw new InvalidFeedXmlException("Unable to parse feedtype from feed", ex);
      }
    }

    private IFeed CreateFeed(FeedType feedtype, string feedxml)
    {
      try
      {
        IFeed returnFeed;
        if (feedtype == FeedType.Atom10)
        {
          returnFeed = new Atom10Feed(feedxml);
        }
        else
        {
          returnFeed = new Rss20Feed(feedxml);
        }

        try
        {
          this.parser.ParseFeed(returnFeed, feedxml);
        }
        catch (System.Xml.XmlException ex)
        {
          throw new InvalidFeedXmlException("The xml for feed is invalid", ex);
        }

        return returnFeed;
      }
      catch (XmlException ex)
      {
        throw new InvalidFeedXmlException("Invalid XML for feed", ex);
      }
    }    
  }
}

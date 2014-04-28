namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using System.Xml;

  using MB.LibraryRss.WebUi.Interfaces;

  using QDFeedParser;
  using QDFeedParser.Xml;

  public class StreamFeedFactory : ICustomFeedFactory
  {
    private readonly IFeedXmlParser parser;

    public StreamFeedFactory(IFeedXmlParser parser)
    {
      this.parser = parser;
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

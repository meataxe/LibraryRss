namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  using CsQuery;

  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Interfaces;

  using QDFeedParser;

  public class TitleService : ITitleService
  {
    private readonly IDownloadService downloadService;
    private readonly ICustomFeedFactory feedFactory;
    private readonly ITitleAnalysisService analysisService;
    private readonly IPersistanceService persistanceService;

    public TitleService(IDownloadService downloadService, ICustomFeedFactory feedFactory, ITitleAnalysisService analysisService, IPersistanceService persistanceService, IFeedSourceFactory feedSourceFactory)
    {
      this.downloadService = downloadService;
      this.feedFactory = feedFactory;
      this.analysisService = analysisService;
      this.persistanceService = persistanceService;

      this.FeedSource = feedSourceFactory.GetFeedSource();      
    }

    public FeedSource FeedSource { get; set; }    

    public List<TitleResult> GetTitles()
    {
      return this.persistanceService.GetLatest();
    }

    public List<TitleResult> RefreshTitles()
    {
      var feedXml = this.FeedSource.SourceType == SearchSource.File
        ? File.ReadAllLines(this.FeedSource.Source).Aggregate(string.Empty, (current, next) => current + "\r\n" + next).Trim()
        : this.downloadService.Download(this.FeedSource.Source);
            
      var feed = this.feedFactory.CreateFeed(feedXml);      

      // check updated tag against db, if newer then continue, else halt
      var lastUpdated = this.persistanceService.GetLastUpdated();
      if (lastUpdated == null || lastUpdated < feed.LastUpdated)
      {
        var titles = this.GetTitlesFromFeed(feed);
        this.persistanceService.Save(titles, feed.LastUpdated);
        return titles;
      }

      return this.GetTitles();
    }    

    private static TitleResult GetTitleResult(BaseFeedItem i)
    {
      return new TitleResult
      {
        Author = i.Author,
        Categories = i.Categories,
        Content = i.Content,
        DatePublished = i.DatePublished,
        Id = i.Id,
        Url = i.Link,
        Title = i.Title
      };
    }

    private static string GetShelfLocations(string raw)
    {
      var locations = raw.Trim().Split('\n').Select(s => s.Trim()).Distinct().OrderBy(s => s);

      return locations.Aggregate(string.Empty, (result, location) => result.Length > 0 ? string.Format("{0}; {1}", result, location) : location);
    }

    private TitleResult GetExtraInfo(TitleResult title, string content)
    {
      var dom = CQ.CreateFragment(content);

      title.Author = !string.IsNullOrEmpty(title.Author) ? title.Author : dom["div.INITIAL_AUTHOR_SRCH"].Text().Trim();
      title.Isbn = dom["div.ISBN"].Text().Trim();
      title.ShelfLocation = GetShelfLocations(dom["table.detailItemTable tr.detailItemsTableRow td:nth-child(2)"].Text());
      title.IsNonFiction = this.analysisService.GetNonFictionStatus(title.ShelfLocation) ? "Yes" : "No";
      title.ShelfLocationScore = this.analysisService.GetStatus(title.ShelfLocation);
      title.SubjectTerms = dom["div.SUBJECT_TERM a"].Select(a => a.GetAttribute("title").Trim()).ToList();

      // A call to this url may not work outside of the pncc domain:
      title.LargeImageUrl = string.Format("https://secure.syndetics.com/index.aspx?type=xw12&client=nlonzsd&upc=&oclc=&isbn={0}/LC.JPG", title.Isbn);
      title.SmallImageUrl = title.LargeImageUrl.Replace("LC.JPG", "SC.JPG");

      return title;
    }

    private List<TitleResult> GetTitlesFromFeed(IFeed feed)
    {      
      var results = feed.Items.OrderBy(i => i.Title).Select(GetTitleResult).ToList();
      var pages = this.downloadService.Download(results.Select(r => r.Url));      

      return results.Select(r => this.GetExtraInfo(r, pages[r.Url])).ToList();
    }          
  }
}

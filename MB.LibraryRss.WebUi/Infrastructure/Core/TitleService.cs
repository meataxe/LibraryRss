namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using CsQuery;

  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Interfaces;

  using QDFeedParser;

  public class TitleService : ITitleService
  {
    private readonly IDownloadService downloadService;
    private readonly ITitleFeedFactory feedFactory;
    private readonly ITitleAnalysisService analysisService;
    private readonly ITitleDataService titleDataService;
    private readonly ISettingDataService settingDataService;

    public TitleService(ITitleDataService titleDataService, ISettingDataService settingDataService, IDownloadService downloadService, ITitleFeedFactory feedFactory, ITitleAnalysisService analysisService)
    {
      this.downloadService = downloadService;
      this.feedFactory = feedFactory;
      this.analysisService = analysisService;
      this.titleDataService = titleDataService;
      this.settingDataService = settingDataService;
    }

    public List<TitleResult> GetTitles()
    {
      return this.titleDataService.GetLatest();
    }

    public List<TitleResult> RefreshTitles(bool force = false)
    {
      if (!force || !this.OkToStart())
      {
        return this.GetTitles();
      }

      this.settingDataService.AddOrUpdateRefreshTaskExecutionStartDate(DateTime.Now);

      try
      {
        var titleFeed = this.feedFactory.DefaultFeed();        
        var titles = this.GetTitlesFromFeed(titleFeed);
        this.titleDataService.Save(titles, titleFeed.LastUpdated);        
      }
      finally
      {
        this.settingDataService.AddOrUpdateRefreshTaskExecutionStartDate(null);
      }

      return this.GetTitles();
    }    

    private static TitleResult GetTitleResult(BaseFeedItem i)
    {
      return new TitleResult
      {
        Author = i.Author,
        Categories = i.Categories.ToList(),
        Content = i.Content,
        DatePublished = i.DatePublished,
        Id = i.Id,
        ExtraInfoUrl = i.Link,        
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
      title.IsFiction = this.analysisService.IsNonFiction(title.ShelfLocation) ? "No" : "Yes";      
      title.SubjectTerms = dom["div.SUBJECT_TERM a"].Select(a => a.GetAttribute("title").Trim()).ToList();
      title.Score = this.analysisService.GetScore(title);

      // A call to this url may not work outside of the pncc domain:
      title.LargeImageUrl = string.Format("https://secure.syndetics.com/index.aspx?type=xw12&client=nlonzsd&upc=&oclc=&isbn={0}/LC.JPG", title.Isbn);
      title.SmallImageUrl = title.LargeImageUrl.Replace("LC.JPG", "SC.JPG");

      title.TitleUrl = string.Format("https://ent.kotui.org.nz/client/en_AU/pn/search/results?qu={0} {1}", title.Title, title.Author);

      return title;
    }

    private List<TitleResult> GetTitlesFromFeed(IFeed feed)
    {      
      var results = feed.Items.OrderBy(i => i.Title).Select(GetTitleResult).ToList();
      var pages = this.downloadService.Download(results.Select(r => r.ExtraInfoUrl));

      return results.Select(r => this.GetExtraInfo(r, pages[r.ExtraInfoUrl])).ToList();
    }  
    
    // Only allow to start if not started, or started >10mins ago
    private bool OkToStart()
    {
      var lastStartDate = this.settingDataService.GetRefreshTaskExecutionStartDate();

      return !lastStartDate.HasValue || (lastStartDate.Value.AddMinutes(10) <= DateTime.Now);
    }   
  }
}

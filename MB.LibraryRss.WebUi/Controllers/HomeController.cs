namespace MB.LibraryRss.WebUi.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Web;
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Interfaces;
  using MB.LibraryRss.WebUi.ViewModel;

  using Newtonsoft.Json;

  public class HomeController : BaseController
  {
    private readonly ITitleService titleService;

    private readonly ITitleDataService titleDataService;

    private readonly ITitleAnalysisService titleAnalysisService;

    public HomeController(ITitleService titleService, ITitleDataService titleDataService, ITitleAnalysisService titleAnalysisService)
    {
      this.titleService = titleService;
      this.titleDataService = titleDataService;
      this.titleAnalysisService = titleAnalysisService;
    }

    public ActionResult Index()
    {
      var d = this.titleDataService.GetDateDataLastRefreshed();      
      
      var message = d.HasValue 
        ? string.Format("@ {0:yyyy-MM-dd HH:mm:ss}", d) 
        : "No data";
      
      var vm = new HomeIndexViewModel
                 {
                   TitleResults = this.titleService.GetTitles(),
                   Message = message
                 };

      return this.View(vm);
    }

    public ActionResult Test()
    {
      var d = this.titleDataService.GetDateDataLastRefreshed();
      var message = d.HasValue
        ? string.Format("@ {0:yyyy-MM-dd HH:mm:ss}", d)
        : "No data";

      var vm = new HomeIndexViewModel
      {
        TitleResults = this.titleService.GetTitles(),
        Message = message
      };

      var scoreList = this.GetScoreList(this.Request.Cookies.Get("mb-libraryrss-home-test-scorelist"));
      foreach (var result in vm.TitleResults)
      {
        result.Score = this.titleAnalysisService.GetScore(result, scoreList);
      }

      return this.View(vm);
    }

    public ActionResult About()
    {
      return this.View();
    }  
  
    private List<TextScore> GetScoreList(HttpCookie cookie)
    {
      const string CookieName = "mb-libraryrss-home-test-scorelist";

      cookie = cookie ?? new HttpCookie(CookieName);
      cookie.Expires = DateTime.Now.AddDays(1000);
      cookie.Path = "/";

      var scores = JsonConvert.DeserializeObject<TextScoreJson>(cookie.Value ?? string.Empty, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });      
      
      if (scores == null)
      {
        scores = new TextScoreJson { TextScores = this.titleAnalysisService.ScoreList };
      }  
      
      cookie.Value = JsonConvert.SerializeObject(scores);
      this.Response.SetCookie(cookie);

      return scores.TextScores;
    }
    
    private class TextScoreJson
    {
      public List<TextScore> TextScores { get; set; }
    }
  }
}

namespace MB.LibraryRss.WebUi.Controllers
{
  using System;
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Interfaces;

  public class HomeController : BaseController
  {
    private readonly ITitleSearchService titleSearchService;

    private readonly IPersistanceService persistanceService;

    public HomeController(ITitleSearchService titleSearchService, IPersistanceService persistanceService)
    {
      this.persistanceService = persistanceService;
      this.titleSearchService = titleSearchService;
    }

    public ActionResult Index()
    {
      try
      {
        return this.View(this.persistanceService.GetLatest());
      }
      catch (Exception)
      {
        return this.View();
      }
    }

    public ActionResult Test()
    {
      return this.View();
    }

    [System.Web.Http.HttpGet, System.Web.Http.HttpPost]
    public JsonResult RefreshTitles()
    {
      // var titles = titleSearchService.GetTitlesFromFile(@"C:\_Marks\Dev\Test\MB.LibraryRss\MB.LibraryRss.WebUi\rss.xml");
      var titles = this.titleSearchService.GetTitlesFromUrl(@"https://ent.kotui.org.nz/client/rss/hitlist/pn/qu=newbks-pn&dt=list&st=PD");

      this.persistanceService.Save(titles);

      // get async thing working.      
      return new JsonResult { Data = titles, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    }
  }
}

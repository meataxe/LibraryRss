namespace MB.LibraryRss.WebUi.Controllers
{
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
      return View(this.persistanceService.GetLatest());
    }

    [System.Web.Http.HttpGet, System.Web.Http.HttpPost]
    public JsonResult RefreshTitles()
    {
      // var titles = titleSearchService.GetTitlesFromFile(@"C:\_Marks\Dev\Test\MB.LibraryRss\MB.LibraryRss.WebUi\rss.xml");
      var titles = titleSearchService.GetTitlesFromUrl(@"https://ent.kotui.org.nz/client/rss/hitlist/pn/qu=newbks-pn&dt=list&st=PD");      

      persistanceService.Save(titles);

      // get async thing working.      
      return new JsonResult { Data = titles, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    }
  }
}

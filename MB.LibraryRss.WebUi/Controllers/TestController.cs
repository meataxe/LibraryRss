namespace MB.LibraryRss.WebUi.Controllers
{
  using System.Web.Http;
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Interfaces;

  public class TestController : ApiController
  {
    private readonly ITitleSearchService titleSearchService;

    private readonly IPersistanceService persistanceService;

    public TestController(ITitleSearchService titleSearchService, IPersistanceService persistanceService)
    {
      this.titleSearchService = titleSearchService;
      this.persistanceService = persistanceService;
    }

    [System.Web.Http.HttpGet, System.Web.Http.HttpPost]
    public JsonResult RefreshTitles()
    {
      //var titles = titleSearchService.GetTitlesFromUri(new Uri(@"https://ent.kotui.org.nz/client/rss/hitlist/pn/qu=newbks-pn&dt=list&st=PD"));
      var titles = titleSearchService.GetTitlesFromFile(@"C:\_Marks\Dev\Test\MB.LibraryRss\MB.LibraryRss.WebUi\rss.xml");
      persistanceService.Save(titles);

      // get async thing working.      
      return new JsonResult { Data = titles, JsonRequestBehavior = JsonRequestBehavior.AllowGet };      
    }
  }
}

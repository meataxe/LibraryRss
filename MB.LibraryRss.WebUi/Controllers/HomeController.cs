namespace MB.LibraryRss.WebUi.Controllers
{
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces;
  using MB.LibraryRss.WebUi.Interfaces;

  public class HomeController : BaseController
  {
    private readonly ITitleService titleService;

    private readonly IDatastoreService datastoreService;

    public HomeController(ITitleService titleService, IDatastoreService datastoreService)
    {
      this.titleService = titleService;
      this.datastoreService = datastoreService;
    }

    public ActionResult Index()
    {
      return this.View(this.titleService.GetTitles());
    }

    public ActionResult About()
    {
      var freeSpace = this.datastoreService.FreeSpace();
      this.ViewBag.FreeSpace = freeSpace;

      return this.View();
    }

    public JsonResult RefreshTitles()
    {
      // get async thing working.      
      this.titleService.RefreshTitles();      
      return this.GetTitles();
    }

    public JsonResult GetTitles()
    {
      return new JsonResult { Data = this.titleService.GetTitles(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    }    
  }
}

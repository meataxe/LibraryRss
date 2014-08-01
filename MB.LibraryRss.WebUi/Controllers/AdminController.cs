namespace MB.LibraryRss.WebUi.Controllers
{
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces;
  using MB.LibraryRss.WebUi.Interfaces;

  public class AdminController : BaseController
  {
    private readonly IDatastoreService datastoreService;

    private readonly ITitleService titleService;

    public AdminController(IDatastoreService datastoreService, ITitleService titleService)
    {
      this.datastoreService = datastoreService;
      this.titleService = titleService;      
    }

    public ActionResult Index()
    {
      var freeSpace = this.datastoreService.FreeSpace();
      this.ViewBag.FreeSpace = freeSpace;

      return this.View();
    }

    public ActionResult RefreshTitles()
    {
      this.titleService.RefreshTitles(true);

      return this.RedirectToAction("Index", "Home");
    }

    public ActionResult InitialiseDatabase()
    {
      //this.datastoreService.EnsureDatabaseIsInitialised(true);

      return this.RedirectToAction("Index");
    }
  }
}

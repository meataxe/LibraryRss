namespace MB.LibraryRss.WebUi.Controllers
{
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces;

  public class AdminController : BaseController
  {
    private readonly IDatastoreService datastoreService;

    public AdminController(IDatastoreService datastoreService)
    {
      this.datastoreService = datastoreService;
    }

    public ActionResult Index()
    {
      return this.View();
    }

    public ActionResult InitialiseDatabase()
    {
      this.datastoreService.EnsureDatabaseIsInitialised(true);

      return this.RedirectToAction("Index");
    }
  }
}

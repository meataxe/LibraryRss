namespace MB.LibraryRss.WebUi.Controllers
{
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Interfaces;

  public class AdminController : BaseController
  {
    private readonly ITitleService titleService;

    private readonly IInitialisationService initialisationService;

    public AdminController(ITitleService titleService, IInitialisationService initialisationService)
    {
      this.titleService = titleService;
      this.initialisationService = initialisationService;
    }

    public ActionResult Index()
    {
      return this.View();
    }

    public ActionResult InitialiseDatabase()
    {
      this.initialisationService.EnsureDatabaseIsInitialised(true);

      return this.Index();
    }
  }
}

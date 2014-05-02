namespace MB.LibraryRss.WebUi.Controllers
{
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Interfaces;

  public class AdminController : BaseController
  {
    private readonly ITitleService titleService;

    public AdminController(ITitleService titleService)
    {
      this.titleService = titleService;
    }

    public ActionResult Index()
    {
      return this.View();
    }
  }
}

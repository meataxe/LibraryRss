namespace MB.LibraryRss.WebUi.Controllers
{
  using System.Web.Mvc;

  using MB.LibraryRss.WebUi.Interfaces;

  public class HomeController : BaseController
  {
    private readonly ITitleService titleService;

    public HomeController(ITitleService titleService)
    {
      this.titleService = titleService;
    }

    public ActionResult Index()
    {
      return this.View(this.titleService.GetTitles());
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

namespace MB.LibraryRss.WebUi.ViewModel
{
  using System.Collections.Generic;

  using MB.LibraryRss.WebUi.Domain;

  public class HomeIndexViewModel
  {
    public List<TitleResult> TitleResults { get; set; }

    public string Message { get; set; }
  }
}

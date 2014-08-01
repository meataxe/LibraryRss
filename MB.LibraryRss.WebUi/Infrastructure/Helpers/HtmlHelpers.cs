namespace MB.LibraryRss.WebUi.Infrastructure.Helpers
{
  using System.Web.Mvc;

  using Newtonsoft.Json;

  public static class HtmlHelpers
  {
    public static MvcHtmlString ToJson(this object obj)
    {
      return new MvcHtmlString(JsonConvert.SerializeObject(obj));
    }    
  }
}
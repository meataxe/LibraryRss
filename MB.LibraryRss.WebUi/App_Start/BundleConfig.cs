namespace MB.LibraryRss.WebUi.App_Start
{
  using System.Web.Optimization;

  public class BundleConfig
  {
    // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
    public static void RegisterBundles(BundleCollection bundles)
    {
      bundles.Add(new ScriptBundle("~/bundles/js/jquery")
        .Include("~/Assets/js/jquery-1.10.2.min.js")
        .Include("~/Assets/js/jquery-cookie.js"));

      bundles.Add(new ScriptBundle("~/bundles/js/bootstrap")
        .Include("~/Assets/js/bootstrap.min.js"));

      bundles.Add(new ScriptBundle("~/bundles/js/html5")
        .IncludeDirectory("~/Assets/js/HTML5_support", "*.js", true));

      bundles.Add(new ScriptBundle("~/bundles/js/app")
        .IncludeDirectory("~/Assets/js/app", "*.js", true));
        

      // This bit allows the use of minified files in debug builds
      bundles.IgnoreList.Clear();
    }
  }
}
namespace MB.LibraryRss.WebUi.Interfaces
{
  using System.Collections.Generic;

  public interface IDownloadService
  {
    string Download(string url);

    Dictionary<string, string> Download(IEnumerable<string> urls);    
  }
}
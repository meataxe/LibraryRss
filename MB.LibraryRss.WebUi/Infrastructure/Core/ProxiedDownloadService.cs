namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Net;
  using System.Threading.Tasks;

  using MB.LibraryRss.WebUi.Interfaces;

  public class ProxiedDownloadService : IDownloadService
  {
    public string Download(string url)
    {
      var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
      request.Proxy = WebRequest.GetSystemWebProxy();
      request.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
      request.KeepAlive = false;

      using (var response = (HttpWebResponse)request.GetResponse())
      {
        return GetResponse(response);
      }      
    }

    public Dictionary<string, string> Download(IEnumerable<string> urls)
    {
      var results = new ConcurrentDictionary<string, string>();
      var distinctUrls = urls.Distinct();

      // Because we're hitting the same domain for all urls, we need to make 
      // sure we're being nice and only hitting it a few times, otherwise it
      // might think we're spamming and rate-limit our requests.
      Parallel.ForEach(
        distinctUrls, 
        new ParallelOptions { MaxDegreeOfParallelism = 10 },
        url => results.TryAdd(url, this.Download(url)));

      return results.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private static string GetResponse(WebResponse response)
    {
      // ReSharper disable AssignNullToNotNullAttribute
      var reader = new StreamReader(response.GetResponseStream());
      
      // ReSharper restore AssignNullToNotNullAttribute
      var responseXml = reader.ReadToEnd();
      return responseXml;
    }
  }
}

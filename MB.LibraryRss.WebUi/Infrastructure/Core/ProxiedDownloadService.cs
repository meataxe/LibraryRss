namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Net;
  using System.Threading;
  using System.Threading.Tasks;

  using MB.LibraryRss.WebUi.Interfaces;

  public class ProxiedDownloadService : IDownloadService
  {
    // For ~300 requests, 10 gives 90s response and 200 gives <15s response.
    private const int MaxConcurrentConnections = 200; 

    public ProxiedDownloadService()
    {
      ThreadPool.SetMinThreads(100, 4);
      System.Net.ServicePointManager.DefaultConnectionLimit = MaxConcurrentConnections;
    }

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

      Parallel.ForEach(
        distinctUrls,
        new ParallelOptions { MaxDegreeOfParallelism = MaxConcurrentConnections },
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

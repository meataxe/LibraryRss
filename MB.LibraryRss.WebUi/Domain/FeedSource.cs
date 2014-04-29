namespace MB.LibraryRss.WebUi.Domain
{
  public enum SearchSource
  {
    Url,
    File
  }

  public class FeedSource
  {
    public string Source { get; set; }

    public SearchSource SourceType { get; set; }
  }
}

namespace MB.LibraryRss.WebUi.Domain
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class TitleResult
  {
    public string Title { get; set; }

    public string Author { get; set; }

    public string Id { get; set; }

    public string Url { get; set; }

    public DateTime DatePublished { get; set; }

    public string Content { get; set; }

    public IList<string> Categories { get; set; }

    public string Isbn { get; set; }

    public List<string> SubjectTerms { get; set; }

    public string SubjectTermsString 
    { 
      get
      {
        return this.SubjectTerms.Aggregate(string.Empty, (current, next) => current + "\r\n" + next).Trim();
      } 
    }

    public string LargeImageUrl { get; set; }

    public string SmallImageUrl { get; set; }

    public byte[] Image { get; set; }

    public string IsNonFiction { get; set; }

    public string ShelfLocation { get; set; }

    public int ShelfLocationScore { get; set; }
  }
}

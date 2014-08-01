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

    public string ExtraInfoUrl { get; set; }

    public string TitleUrl { get; set; }

    public DateTime DatePublished { get; set; }

    public string Content { get; set; }

    public List<string> Categories { get; set; }

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

    public string IsFiction { get; set; }

    public string ShelfLocation { get; set; }

    public int Score { get; set; }

    public string GetTextFieldsToScore()
    {
      var categories = this.Categories.Aggregate(string.Empty, (current, category) => current + (category ?? string.Empty)).Trim();
      var terms = this.SubjectTerms.Aggregate(string.Empty, (current, term) => current + (term ?? string.Empty)).Trim();

      return string.Format("{0} {1} {2} {3} {4} {5}", this.Title, this.Author, this.Content, categories, terms, this.ShelfLocation);
    }
  }
}

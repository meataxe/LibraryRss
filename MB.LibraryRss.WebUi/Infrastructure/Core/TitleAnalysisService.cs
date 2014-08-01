namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using System.Collections.Generic;
  using System.Linq;

  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Interfaces;

  public class TitleAnalysisService : ITitleAnalysisService
  {
    public TitleAnalysisService()
    {
      this.ScoreList = new List<TextScore>
      {
        new TextScore { Text = "Anime", Score = -1 },
        new TextScore { Text = "Large Print", Score = -1 },
        new TextScore { Text = "Linton Library", Score = -1 },
        new TextScore { Text = "Children's Zone", Score = -1 },
        new TextScore { Text = "Community Languages", Score = -1 },
        new TextScore { Text = "Books on CD", Score = -1 },
        new TextScore { Text = "Second Floor", Score = -1 },
        new TextScore { Text = "Romantic Writing", Score = -1 },
        new TextScore { Text = "console games", Score = -1 },    
        new TextScore { Text = "Animals-Non Fiction Zone", Score = -1 },
        new TextScore { Text = "Arts and Crafts-Non Fiction Zone", Score = -1 },
        new TextScore { Text = "Beliefs-Non Fiction Zone", Score = -1 },
        new TextScore { Text = "Environment and Land Use - Non Fiction Zone", Score = -1 },
        new TextScore { Text = "Food and Drink - Non Fiction Zone", Score = -1 },
        new TextScore { Text = "Get Well Bags - ask at Children's desk", Score = -1 },
        new TextScore { Text = "Health and Well being-Non Fiction Zone", Score = -1 },
        new TextScore { Text = "House and Garden - Non Fiction Zone", Score = -1 },
        new TextScore { Text = "Jigsaws-Fiction Zone", Score = -1 },
        new TextScore { Text = "Literature-Fiction Zone", Score = -1 },
        new TextScore { Text = "Music and Performing Arts Books - Sound+Vision", Score = -1 },
        new TextScore { Text = "Relationships - Youth Collection", Score = -1 },
        new TextScore { Text = "World Languages - Learn a Language", Score = -1 },
        new TextScore { Text = "Youth Non-Fiction - Youth Collection", Score = -1 },
        new TextScore { Text = "Graphic Novels - Youth Collection", Score = -1 },
        new TextScore { Text = "Classic Fiction Books-Fiction Zone", Score = -1 },
        new TextScore { Text = "Graphic Novels - Childrens Zone", Score = -1 },
        new TextScore { Text = "Horror - Youth Collection", Score = -1 },
        new TextScore { Text = "Music CDs", Score = -1 },
        new TextScore { Text = "TV Show DVDs", Score = -1 },
        new TextScore { Text = "Sci Fi", Score = 1 },
        new TextScore { Text = "Sci-Fi", Score = 1 },
        new TextScore { Text = "SciFi", Score = 1 }
      };
    }

    public List<TextScore> ScoreList { get; private set; }

    public int GetScore(TitleResult title)
    {
      return this.GetScore(title, this.ScoreList);
    }

    public int GetScore(TitleResult title, List<TextScore> scoreList)
    {
      var textToScore = title.GetTextFieldsToScore().ToLower();

      return scoreList.Where(ts => textToScore.Contains(ts.Text.ToLower())).Sum(ts => ts.Score);

      /*
      return 
        Blacklist.Where(pair => shelfLocation.ToLowerInvariant().Contains(pair.Key.ToLowerInvariant())).Sum(pair => pair.Value) 
        + Whitelist.Where(pair => shelfLocation.ToLowerInvariant().Contains(pair.Key.ToLowerInvariant())).Sum(pair => pair.Value);
       * 
       */
    }

    public bool IsNonFiction(string shelfLocation)
    {
      var zones = new[] { "non fiction", "nonfiction", "non-fiction" };

      return zones.Any(zone => shelfLocation.ToLowerInvariant().Contains(zone.ToLowerInvariant()));
    }
  }
}
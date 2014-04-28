namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using System.Collections.Generic;
  using System.Linq;

  using MB.LibraryRss.WebUi.Interfaces;

  public class TitleAnalysisService : ITitleAnalysisService
  {
    private static readonly Dictionary<string, int> Blacklist = new Dictionary<string, int>
      {
        { "Large Print", -1 },
        { "Linton Library", -1 },
        { "Children's Zone", -1 },
        { "Community Languages", -1 },
        { "Books on CD", -1 },
        { "Second Floor", -1 },
        { "Romantic Writing", -1 },
        { "console games", -1 },    
        { "Animals-Non Fiction Zone", -1 },
        { "Arts and Crafts-Non Fiction Zone", -1 },
        { "Beliefs-Non Fiction Zone", -1 },
        { "Environment and Land Use - Non Fiction Zone", -1 },
        { "Food and Drink - Non Fiction Zone", -1 },
        { "Get Well Bags - ask at Children's desk", -1 },
        { "Health and Well being-Non Fiction Zone", -1 },
        { "House and Garden - Non Fiction Zone", -1 },
        { "Jigsaws-Fiction Zone", -1 },
        { "Literature-Fiction Zone", -1 },
        { "Music and Performing Arts Books - Sound+Vision", -1 },
        { "Relationships - Youth Collection", -1 },
        { "World Languages - Learn a Language", -1 },
        { "Youth Non-Fiction - Youth Collection", -1 }                                                     
      };

    private static readonly Dictionary<string, int> Whitelist = new Dictionary<string, int>
      {
        { "Sci Fi", 1 },
        { "Sci-Fi", 1 },
        { "SciFi", 1 }
      };
    
    public int GetStatus(string shelfLocation)
    {
      return 
        Blacklist.Where(pair => shelfLocation.ToLowerInvariant().Contains(pair.Key.ToLowerInvariant())).Sum(pair => pair.Value) 
        + Whitelist.Where(pair => shelfLocation.ToLowerInvariant().Contains(pair.Key.ToLowerInvariant())).Sum(pair => pair.Value);
    }

    public bool GetNonFictionStatus(string shelfLocation)
    {
      var zones = new[] { "non fiction", "nonfiction", "non-fiction" };

      return zones.Any(zone => shelfLocation.ToLowerInvariant().Contains(zone.ToLowerInvariant()));
    }
  }
}
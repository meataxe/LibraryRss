namespace MB.LibraryRss.WebUi.Interfaces
{
  using System.Collections.Generic;

  using MB.LibraryRss.WebUi.Domain;

  public interface ITitleAnalysisService
  {
    List<TextScore> ScoreList { get; }

    int GetScore(TitleResult title);

    int GetScore(TitleResult title, List<TextScore> scoreList);

    bool IsNonFiction(string shelfLocation);
  }
}
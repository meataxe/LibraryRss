namespace MB.LibraryRss.WebUi.Interfaces
{
  public interface ITitleAnalysisService
  {
    int GetStatus(string shelfLocation);

    bool GetNonFictionStatus(string shelfLocation);
  }
}
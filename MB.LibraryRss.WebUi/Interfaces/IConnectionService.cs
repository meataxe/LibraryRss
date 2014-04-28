namespace MB.LibraryRss.WebUi.Interfaces
{
  using System.Data.SqlClient;

  public interface IConnectionService
  {
    string NameOrConnectionString();

    SqlConnection GetConnection();
  }
}
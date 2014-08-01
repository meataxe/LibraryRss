namespace MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces
{
  using System.Data.SqlClient;

  public interface IConnectionService
  {
    string NameOrConnectionString();

    SqlConnection GetConnection();
  }
}
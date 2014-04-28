namespace MB.LibraryRss.WebUi.Infrastructure.Core
{
  using System.Data.SqlClient;
  using System.Web.Configuration;

  using MB.LibraryRss.WebUi.Interfaces;

  public class ConnectionService : IConnectionService
  {
    public string NameOrConnectionString()
    {
      return WebConfigurationManager.ConnectionStrings["DataConnectionString"].ConnectionString;      
    }

    public SqlConnection GetConnection()
    {
      return new SqlConnection(this.NameOrConnectionString());
    } 
  }
}
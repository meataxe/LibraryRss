namespace MB.LibraryRss.WebUi.Infrastructure.Orm
{
  using System.Collections.Generic;
  using System.Data;
  using System.Data.SqlClient;
  using System.Linq;

  using MB.LibraryRss.WebUi.Domain;
  using MB.LibraryRss.WebUi.Interfaces;

  using Newtonsoft.Json;

  public class PersistanceService : IPersistanceService
  {
    private readonly IConnectionService connectionService;

    private readonly IInitialisationService initialisationService;

    public PersistanceService(IConnectionService connectionService, IInitialisationService initialisationService)
    {
      this.connectionService = connectionService;
      this.initialisationService = initialisationService;      
    }

    public void Save(List<TitleResult> results)
    {
      this.initialisationService.EnsureDatabaseIsInitialised();

      using (var connection = this.connectionService.GetConnection())
      {
        connection.Open();
        DeleteElements(connection);

        foreach (var result in results)
        {
          InsertElement(connection, JsonConvert.SerializeObject(result));
        }
      }
    }

    public List<TitleResult> GetLatest()
    {
      this.initialisationService.EnsureDatabaseIsInitialised();

      var results = new List<TitleResult>();      
      using (var connection = this.connectionService.GetConnection())
      {
        connection.Open();
        results.AddRange(ReadElements(connection).Select(JsonConvert.DeserializeObject<TitleResult>));
      }

      return results;
    }

    private static void InsertElement(SqlConnection connection, string titleData)
    {
      using (var cmd = new SqlCommand("InsertElement", connection) { CommandType = CommandType.StoredProcedure })
      {
        cmd.Parameters.Add(new SqlParameter("@Data", titleData));
        cmd.ExecuteNonQuery();
      }
    }

    private static void DeleteElements(SqlConnection connection)
    {
      using (var cmd = new SqlCommand("DeleteElements", connection) { CommandType = CommandType.StoredProcedure })
      {
        cmd.ExecuteNonQuery();
      }
    }

    private static IEnumerable<string> ReadElements(SqlConnection connection)
    {
      var results = new List<string>();
      using (var cmd = new SqlCommand("ReadElements", connection) { CommandType = CommandType.StoredProcedure })
      {
        using (var rdr = cmd.ExecuteReader())
        {
          while (rdr.Read())
          {
            results.Add(rdr["Data"].ToString());
          }
        }
      }

      return results;
    }
  }
}

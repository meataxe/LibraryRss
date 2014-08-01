namespace MB.LibraryRss.WebUi.Infrastructure.Orm
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.SqlClient;

  using MB.LibraryRss.WebUi.Infrastructure.Orm.Interfaces;

  public class SqlService : IDatastoreService
  {
    private readonly string nameOrConnectionString;

    public SqlService(IConnectionService connectionService)
    {
      this.nameOrConnectionString = connectionService.NameOrConnectionString();
    }

    /*
     * The purpose of this method is to test if the db contains the required objects
     * and if not, to create them.
     */
    public void EnsureDatabaseIsInitialised(bool forceInitialisation = false)
    {
      using (var connection = this.GetConnection())
      {
        connection.Open();

        if (!forceInitialisation && IsInitialised(connection))
        {
          return;
        }
        
        // Allow resetting the db outside of normal operation, in case of schema changes
        if (forceInitialisation)
        {
          ClearDatabase(connection);
        }

        // Best case: db is initialised and ready to use
        if (IsInitialised(connection))
        {
          return;
        }

        // Next case: db is uninitialised and ready to initialise
        InitialiseDatabase(connection);
        if (IsInitialised(connection))
        {
          return;
        }

        // Last case: db is partially initialised and needs to be cleared out first
        ClearDatabase(connection);
        InitialiseDatabase(connection);
        if (IsInitialised(connection))
        {
          return;
        }
        
        // Failure: Unable to init, no exceptions thrown so far, can't further diagnose automatically
        throw new ApplicationException("Unable to initialise database");                
      }
    }

    public decimal FreeSpace()
    {
      const string Cmd =
        "SELECT database_name = DB_NAME(database_id), log_size_mb = CAST(SUM(CASE WHEN type_desc = 'LOG' THEN size END) * 8. / 1024 AS DECIMAL(8,2)), row_size_mb = CAST(SUM(CASE WHEN type_desc = 'ROWS' THEN size END) * 8. / 1024 AS DECIMAL(8,2)), total_size_mb = CAST(SUM(size) * 8. / 1024 AS DECIMAL(8,2)) FROM sys.master_files WITH(NOWAIT) WHERE database_id = DB_ID() GROUP BY database_id";
      
      using (var connection = this.GetConnection())
      {
        connection.Open();
        using (var cmd = new SqlCommand(Cmd, connection) { CommandType = CommandType.Text })
        {
          cmd.ExecuteScalar();

          using (var rdr = cmd.ExecuteReader())
          {
            rdr.Read();

            return !rdr.HasRows ? 0 : decimal.Parse(rdr[3].ToString());
          }
        }
      }
    }
  
    private static bool IsInitialised(SqlConnection connection)
    {
      using (var cmd = new SqlCommand("SELECT COUNT(*) AS TableExists FROM sys.tables WHERE name = 'Feeds'", connection) { CommandType = CommandType.Text })
      {
        cmd.ExecuteScalar();

        using (var rdr = cmd.ExecuteReader())
        {
          rdr.Read();
          return int.Parse(rdr[0].ToString()) == 1;
        }
      }
    }

    private static void ExecuteNonQuery(SqlConnection connection, SqlTransaction transaction, string sql)
    {
      using (var cmd = new SqlCommand(sql, connection) { CommandType = CommandType.Text, Transaction = transaction })
      {
        cmd.ExecuteNonQuery();
      }
    }

    private static IEnumerable<string> SplitStatements(string sql)
    {
      return sql.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static void InitialiseDatabase(SqlConnection connection)
    {
      var sql = Properties.Resources.ResourceManager.GetString("InitSql");

      using (var transaction = connection.BeginTransaction())
      {
        foreach (var statement in SplitStatements(sql))
        {
          ExecuteNonQuery(connection, transaction, statement);
        }

        transaction.Commit();
      }
    }

    private static void ClearDatabase(SqlConnection connection)
    {
      var sql = Properties.Resources.ResourceManager.GetString("DropSql");

      using (var transaction = connection.BeginTransaction())
      {
        foreach (var statement in SplitStatements(sql))
        {
          ExecuteNonQuery(connection, transaction, statement);
        }

        transaction.Commit();
      }

      // Might not actually be cleared, but the app should treat it as such.      
    }

    private SqlConnection GetConnection()
    {
      return new SqlConnection(this.nameOrConnectionString);
    }   
  }  
}
namespace MB.LibraryRss.WebUi.Infrastructure.Orm
{
  using System;
  using System.Data;
  using System.Data.SqlClient;
  using System.Diagnostics.CodeAnalysis;
  using System.Text;
  using System.Transactions;

  using MB.LibraryRss.WebUi.Interfaces;

  using Microsoft.SqlServer.Management.Common;
  using Microsoft.SqlServer.Management.Smo;

  using IsolationLevel = System.Transactions.IsolationLevel;

  public class InitialisationService : IInitialisationService
  {
    private readonly IConnectionService connectionService;

    private readonly string initSql;

    private readonly string clearSql;

    private bool isInitialised;

    public InitialisationService(IConnectionService connectionService)
    {
      this.connectionService = connectionService;
      
      this.initSql = GetInitSql();
      this.clearSql = GetClearSql();
    }

    /*
     * The purpose of this method is to test if the db contains the required objects
     * and if not, to create them.
     */
    public void EnsureDatabaseIsInitialised()
    {
      if (this.isInitialised)
      {        
        return;
      }

      using (var connection = this.connectionService.GetConnection())
      {
        connection.Open();

        // Best case: db is initialised and ready to use
        this.RefreshInitialisationStatus(connection);
        if (this.isInitialised)
        {
          return;
        }

        // Next case: db is uninitialised and ready to initialise
        this.InitialiseDatabase(connection);
        if (this.isInitialised)
        {
          return;
        }

        // Last case: db is partially initialised and needs to be cleared out first
        this.ClearDatabase(connection);
        this.InitialiseDatabase(connection);
        if (this.isInitialised)
        {
          return;
        }
        
        // Failure: Unable to init, no exceptions thrown so far, can't further diagnose automatically
        throw new ApplicationException("Unable to initialise database");                
      }
    }

    private static bool IsDatabaseInitialised(SqlConnection connection)
    {
      using (var cmd = new SqlCommand("SELECT COUNT(*) AS TableExists FROM sys.tables WHERE name = 'Element'", connection) { CommandType = CommandType.Text })
      {
        cmd.ExecuteScalar();

        using (var rdr = cmd.ExecuteReader())
        {
          rdr.Read();
          return int.Parse(rdr[0].ToString()) == 1;
        }
      }
    }

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1122:UseStringEmptyForEmptyStrings",
      Justification = "Reviewed. Suppression is OK here.")]
    private static string GetClearSql()
    {
      var b = new StringBuilder();
      b.AppendLine("IF OBJECT_ID('Element') IS NOT NULL");
      b.AppendLine("  BEGIN");
      b.AppendLine("    DROP TABLE [Element]");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("");
      b.AppendLine("IF OBJECT_ID('DeleteElements') IS NOT NULL");
      b.AppendLine("  BEGIN");
      b.AppendLine("    DROP PROCEDURE [DeleteElements]");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("");
      b.AppendLine("IF OBJECT_ID('InsertElement') IS NOT NULL");
      b.AppendLine("  BEGIN");
      b.AppendLine("    DROP PROCEDURE [InsertElement]");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("");
      b.AppendLine("IF OBJECT_ID('ReadElements') IS NOT NULL");
      b.AppendLine("  BEGIN");
      b.AppendLine("    DROP PROCEDURE [ReadElements]");
      b.AppendLine("  END");
      b.AppendLine("GO");

      return b.ToString();
    }

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1122:UseStringEmptyForEmptyStrings", Justification = "Reviewed. Suppression is OK here.")]
    private static string GetInitSql()
    {
      var b = new StringBuilder();
      b.AppendLine("SET NUMERIC_ROUNDABORT OFF");
      b.AppendLine("GO");
      b.AppendLine("SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON");
      b.AppendLine("GO");
      b.AppendLine("IF EXISTS ( SELECT");
      b.AppendLine("              *");
      b.AppendLine("            FROM");
      b.AppendLine("              tempdb..sysobjects");
      b.AppendLine("            WHERE");
      b.AppendLine("              id = OBJECT_ID('tempdb..#tmpErrors') )");
      b.AppendLine("  DROP TABLE #tmpErrors");
      b.AppendLine("GO");
      b.AppendLine("CREATE TABLE #tmpErrors ( Error INT )");
      b.AppendLine("GO");
      b.AppendLine("SET XACT_ABORT ON");
      b.AppendLine("GO");
      b.AppendLine("SET TRANSACTION ISOLATION LEVEL SERIALIZABLE");
      b.AppendLine("GO");
      b.AppendLine("BEGIN TRANSACTION");
      b.AppendLine("GO");
      b.AppendLine("PRINT N'Creating [dbo].[Element]'");
      b.AppendLine("GO");
      b.AppendLine("CREATE TABLE [dbo].[Element]");
      b.AppendLine("  (");
      b.AppendLine("    [Id] [int] NOT NULL");
      b.AppendLine("               IDENTITY(1, 1),");
      b.AppendLine("    [Data] [text] COLLATE Latin1_General_CI_AS");
      b.AppendLine("                  NOT NULL,");
      b.AppendLine("    [Inserted] [datetime] NOT NULL");
      b.AppendLine("                          CONSTRAINT [DF_ChildElement_Inserted] DEFAULT ( GETDATE() )");
      b.AppendLine("  )");
      b.AppendLine("GO");
      b.AppendLine("IF @@ERROR <> 0");
      b.AppendLine("  AND @@TRANCOUNT > 0");
      b.AppendLine("  ROLLBACK TRANSACTION");
      b.AppendLine("GO");
      b.AppendLine("IF @@TRANCOUNT = 0");
      b.AppendLine("  BEGIN");
      b.AppendLine("    INSERT  INTO #tmpErrors");
      b.AppendLine("            ( Error )");
      b.AppendLine("            SELECT");
      b.AppendLine("              1");
      b.AppendLine("    BEGIN TRANSACTION");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("PRINT N'Creating primary key [PK_ChildElement] on [dbo].[Element]'");
      b.AppendLine("GO");
      b.AppendLine("ALTER TABLE [dbo].[Element] ADD CONSTRAINT [PK_ChildElement] PRIMARY KEY CLUSTERED  ([Id])");
      b.AppendLine("GO");
      b.AppendLine("IF @@ERROR <> 0");
      b.AppendLine("  AND @@TRANCOUNT > 0");
      b.AppendLine("  ROLLBACK TRANSACTION");
      b.AppendLine("GO");
      b.AppendLine("IF @@TRANCOUNT = 0");
      b.AppendLine("  BEGIN");
      b.AppendLine("    INSERT  INTO #tmpErrors");
      b.AppendLine("            ( Error )");
      b.AppendLine("            SELECT");
      b.AppendLine("              1");
      b.AppendLine("    BEGIN TRANSACTION");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("PRINT N'Creating [dbo].[InsertElement]'");
      b.AppendLine("GO");
      b.AppendLine("CREATE PROCEDURE [dbo].[InsertElement] ( @Data TEXT )");
      b.AppendLine("AS");
      b.AppendLine("  BEGIN");
      b.AppendLine("    SET NOCOUNT ON;");
      b.AppendLine("");
      b.AppendLine("    INSERT  INTO [Element]");
      b.AppendLine("            ( [Data] )");
      b.AppendLine("    VALUES");
      b.AppendLine("            ( @Data )");
      b.AppendLine("");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("IF @@ERROR <> 0");
      b.AppendLine("  AND @@TRANCOUNT > 0");
      b.AppendLine("  ROLLBACK TRANSACTION");
      b.AppendLine("GO");
      b.AppendLine("IF @@TRANCOUNT = 0");
      b.AppendLine("  BEGIN");
      b.AppendLine("    INSERT  INTO #tmpErrors");
      b.AppendLine("            ( Error )");
      b.AppendLine("            SELECT");
      b.AppendLine("              1");
      b.AppendLine("    BEGIN TRANSACTION");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("PRINT N'Creating [dbo].[DeleteElements]'");
      b.AppendLine("GO");
      b.AppendLine("CREATE PROCEDURE [dbo].[DeleteElements]");
      b.AppendLine("AS");
      b.AppendLine("  BEGIN");
      b.AppendLine("    SET NOCOUNT ON;");
      b.AppendLine("");
      b.AppendLine("    DELETE FROM");
      b.AppendLine("      [Element]");
      b.AppendLine("");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("IF @@ERROR <> 0");
      b.AppendLine("  AND @@TRANCOUNT > 0");
      b.AppendLine("  ROLLBACK TRANSACTION");
      b.AppendLine("GO");
      b.AppendLine("IF @@TRANCOUNT = 0");
      b.AppendLine("  BEGIN");
      b.AppendLine("    INSERT  INTO #tmpErrors");
      b.AppendLine("            ( Error )");
      b.AppendLine("            SELECT");
      b.AppendLine("              1");
      b.AppendLine("    BEGIN TRANSACTION");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("PRINT N'Creating [dbo].[ReadElements]'");
      b.AppendLine("GO");
      b.AppendLine("CREATE PROCEDURE [dbo].[ReadElements]");
      b.AppendLine("AS");
      b.AppendLine("  BEGIN");
      b.AppendLine("    SET NOCOUNT ON;");
      b.AppendLine("");
      b.AppendLine("    SELECT");
      b.AppendLine("      Data,");
      b.AppendLine("      Inserted");
      b.AppendLine("    FROM");
      b.AppendLine("      [Element]");
      b.AppendLine("");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("IF @@ERROR <> 0");
      b.AppendLine("  AND @@TRANCOUNT > 0");
      b.AppendLine("  ROLLBACK TRANSACTION");
      b.AppendLine("GO");
      b.AppendLine("IF @@TRANCOUNT = 0");
      b.AppendLine("  BEGIN");
      b.AppendLine("    INSERT  INTO #tmpErrors");
      b.AppendLine("            ( Error )");
      b.AppendLine("            SELECT");
      b.AppendLine("              1");
      b.AppendLine("    BEGIN TRANSACTION");
      b.AppendLine("  END");
      b.AppendLine("GO");
      b.AppendLine("PRINT N'Altering permissions on [dbo].[InsertElement]'");
      b.AppendLine("GO");
      b.AppendLine("GRANT EXECUTE ON  [dbo].[InsertElement] TO [public]");
      b.AppendLine("GO");
      b.AppendLine("PRINT N'Altering permissions on [dbo].[ReadElements]'");
      b.AppendLine("GO");
      b.AppendLine("GRANT EXECUTE ON  [dbo].[ReadElements] TO [public]");
      b.AppendLine("GO");
      b.AppendLine("PRINT N'Altering permissions on [dbo].[DeleteElements]'");
      b.AppendLine("GO");
      b.AppendLine("GRANT EXECUTE ON  [dbo].[DeleteElements] TO [public]");
      b.AppendLine("GO");
      b.AppendLine("IF EXISTS ( SELECT");
      b.AppendLine("              *");
      b.AppendLine("            FROM");
      b.AppendLine("              #tmpErrors )");
      b.AppendLine("  ROLLBACK TRANSACTION");
      b.AppendLine("GO");
      b.AppendLine("IF @@TRANCOUNT > 0");
      b.AppendLine("  BEGIN");
      b.AppendLine("    PRINT 'The database update succeeded'");
      b.AppendLine("    COMMIT TRANSACTION");
      b.AppendLine("  END");
      b.AppendLine("ELSE");
      b.AppendLine("  PRINT 'The database update failed'");
      b.AppendLine("GO");
      b.AppendLine("DROP TABLE #tmpErrors");
      b.AppendLine("GO");

      return b.ToString();
    }

    private static void ExecuteTransactionScopedNonQuery(SqlConnection connection, string sql)
    {
      var options = new TransactionOptions
      {
        IsolationLevel = IsolationLevel.ReadCommitted,
        Timeout = TimeSpan.FromMinutes(1)
      };

      using (var transaction = new TransactionScope(TransactionScopeOption.Required, options))
      {
        var server = new Server(new ServerConnection(connection));

        server.ConnectionContext.SqlConnectionObject.EnlistTransaction(Transaction.Current);
        server.ConnectionContext.ExecuteNonQuery(sql);

        transaction.Complete();
      }
    }

    private void RefreshInitialisationStatus(SqlConnection connection)
    {
      this.isInitialised = IsDatabaseInitialised(connection);
    }

    private void InitialiseDatabase(SqlConnection connection)
    {
      ExecuteTransactionScopedNonQuery(connection, this.initSql);
      this.RefreshInitialisationStatus(connection);
    }

    private void ClearDatabase(SqlConnection connection)
    {
      ExecuteTransactionScopedNonQuery(connection, this.clearSql);
      
      // Might not actually be cleared, but the app should treat it as such.
      this.isInitialised = false;
    }    
  }
}

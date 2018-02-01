using Citolab.Repository.Options;

namespace Citolab.Repository.SqlServer.Options
{
    public class SqlServerDatabaseOptions : ISqlServerDatabaseOptions
    {
        public string DatabaseName { get; }
        public string ConnectionString { get; }
 
        public SqlServerDatabaseOptions(string databaseName, string connectionString)
        {
            DatabaseName = databaseName;
            ConnectionString = connectionString;
        }

        public SqlServerDatabaseOptions(string databaseName, string connectionString, bool flagDelete, bool timeLoggingEnabled) : this(databaseName, connectionString)
        {
            FlagDelete = flagDelete;
            TimeLoggingEnabled = timeLoggingEnabled;
        }

        public bool FlagDelete { get; set; } = true;
        public bool TimeLoggingEnabled { get; set; }
    }
}


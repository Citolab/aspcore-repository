using System;
using System.Collections.Generic;
using System.Text;
using Citolab.Repository.Model;
using Citolab.Repository.Options;
using Microsoft.Extensions.Caching.Memory;

namespace Citolab.Repository.Options
{
    public class MongoDatabaseOptions : IMongoDatabaseOptions
    {
        public string DatabaseName { get; }
        public string ConnectionString { get; }

        public MongoDatabaseOptions(string databaseName, string connectionString)
        {
            DatabaseName = databaseName;
            ConnectionString = connectionString;
        }

        public bool FlagDelete { get; set; }
        public bool TimeLoggingEnabled { get; set; }
    }
}

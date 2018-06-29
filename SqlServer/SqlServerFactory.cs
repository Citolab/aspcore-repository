using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Citolab.Repository.Decorators;
using System.Data.SqlClient;
using Citolab.Repository.Options;

namespace Citolab.Repository.SqlServer
{
    public class SqlServerFactory : RepositoryFactoryBase
    {
        private readonly ILoggedInUserProvider _loggedInUserProvider;
        public static string ConnectionString = "";
        private readonly SqlDatabaseContext _dbContext;
        private readonly ILogger _logger;

        public ConcurrentDictionary<Type, object> Repositories { get; set; }
        public SqlServerFactory(IMemoryCache memoryCache, ILoggerFactory loggerFactory,
            IRepositoryOptions options, ILoggedInUserProvider loggedInUserProvider) : base(memoryCache, loggerFactory, options)
        {
            if (!(options is ISqlServerDatabaseOptions sqlServerOptions)) throw new Exception("Options should be of type IMongoDatabaseOptions");
            _loggedInUserProvider = loggedInUserProvider;
            _dbContext = new SqlDatabaseContext();
            _logger = loggerFactory.CreateLogger(GetType());
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(environment))
            {
                environment = "Development";
            }

            var connection =
                new SqlConnectionStringBuilder(sqlServerOptions.ConnectionString)
                {
                    InitialCatalog = $"{sqlServerOptions.DatabaseName}-{environment}"
                };
            ConnectionString = connection.ConnectionString;
            Repositories = new ConcurrentDictionary<Type, object>();
        }


        public override IRepository<T> GetRepository<T>()
        {
            _dbContext.Set<T>();
            var sqlServerRepository = new FlagAsDeletedDecorator<T>(MemoryCache,
                new FillDefaultValueDecorator<T>(MemoryCache,
                    new CacheDecorator<T>(MemoryCache, false,
                        new SqlServerRepository<T>(LoggerFactory, _dbContext)), _loggedInUserProvider));
            if (LogTime)
            {
                var timeLoggedMongoRepository =
                    new LogTimeDecorator<T>(MemoryCache, sqlServerRepository, _logger);
                Repositories.TryAdd(typeof(T), timeLoggedMongoRepository);
            }
            else
            {
                Repositories.TryAdd(typeof(T), sqlServerRepository);
            }
            return sqlServerRepository;

        }

    }
}

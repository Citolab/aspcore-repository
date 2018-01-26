using System;
using Citolab.Repository.Helpers;
using Citolab.Repository.Mongo;
using Citolab.Repository.NoAction;
using Citolab.Repository.Options;
using Citolab.Repository.SqlServer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;

namespace Citolab.Repository.Extensions
{
    public static class Extensions
    {
        public static void AddInMemoryRepository(this IServiceCollection services) =>
            services.AddRepository(new InMemoryDatabaseOptions());

        public static void AddMongoRepository(this IServiceCollection services, string databaseName, string connectionString) =>
            services.AddRepository(new MongoDatabaseOptions(databaseName, connectionString));

        public static void AddSqlServerRepository(this IServiceCollection services, string databaseName, string connectionString) =>
            services.AddRepository(new SqlServerDatabaseOptions(databaseName, connectionString));


        private static void AddRepository(this IServiceCollection services, IRepositoryOptions options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddMemoryCache();
            services.AddLogging();
            services.AddSingleton(options);
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRepositoryFactory, SqlServerFactory>();
            // TryAdd will only add if there isn't yet a ILoggedInUserProvider registered.
            // if someone wants to use its own ILoggedInUserProvider, it should be added before calling this function.
            services.TryAddScoped<ILoggedInUserProvider, NoLoggedInUser>();
            switch (options)
            {
                case IInMemoryDatabaseOptions _:
                    {
                        services.AddSingleton<IRepositoryFactory, NoActionFactory>();
                        break;
                    }
                case IMongoDatabaseOptions _:
                    {
                        services.AddSingleton<IRepositoryFactory, MongoFactory>();
                        break;
                    }
                case ISqlServerDatabaseOptions _:
                    {
                        services.AddSingleton<IRepositoryFactory, SqlServerFactory>();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException("Unsupported database");
            }
        }

        public static void AddEventStore<T>(this IServiceCollection services, IEventStoreOptions options)
        {
            services.AddMemoryCache();
            services.AddLogging();
            services.AddSingleton(options);
            switch (options)
            {
                case IMongoEventStoreOptions _:
                    {
                        BsonSerializer.RegisterSerializer(typeof(DateTime), DateTimeSerializer.LocalInstance);
                        services.AddSingleton<IEventStore<T>, MongoEventStore<T>>();
                        break;
                    }
                case IInMemoryEventStoreOptions _:
                    {
                        services.AddSingleton<IEventStore<T>, MongoEventStore<T>>();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException("Unsupported database");
            }
        }

        /// <summary>
        ///     Deep clone using JSON serialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toClone"></param>
        /// <returns></returns>
        public static T Clone<T>(this T toClone) where T : class =>
            JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(toClone));        
    }
}

using System;
using Citolab.Repository.Helpers;
using Citolab.Repository.Mongo.Options;
using Citolab.Repository.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

namespace Citolab.Repository.Mongo.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddMongoRepository(this IServiceCollection services, string databaseName, string connectionString)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddMemoryCache();
            services.AddLogging();
            services.AddSingleton<IRepositoryOptions>(new MongoDatabaseOptions(databaseName, connectionString));
            // TryAdd will only add if there isn't yet a ILoggedInUserProvider registered.
            // if someone wants to use its own ILoggedInUserProvider, it should be added before calling this function.
            services.TryAddScoped<ILoggedInUserProvider, NoLoggedInUser>();
            services.AddSingleton<IRepositoryFactory, MongoFactory>();
            return services;
        }
    }
}

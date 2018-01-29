using System;
using Citolab.Repository.Helpers;
using Citolab.Repository.Mongo.Options;
using Citolab.Repository.NoAction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

namespace Citolab.Repository.Mongo.Extensions
{
    public static class Extensions
    {
        public static void AddMongoRepository(this IServiceCollection services, string databaseName, string connectionString)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddMemoryCache();
            services.AddLogging();
            services.AddSingleton(new MongoDatabaseOptions(databaseName, connectionString));
            // TryAdd will only add if there isn't yet a ILoggedInUserProvider registered.
            // if someone wants to use its own ILoggedInUserProvider, it should be added before calling this function.
            services.TryAddScoped<ILoggedInUserProvider, NoLoggedInUser>();
            services.AddSingleton<IRepositoryFactory, MongoFactory>();
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

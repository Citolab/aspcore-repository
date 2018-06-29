using System;
using Citolab.Repository.Mongo.Options;
using Citolab.Repository.Options;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<IRepositoryFactory, MongoFactory>();
            return services;
        }
    }
}

using System;
using Citolab.Repository.NoAction;
using Citolab.Repository.Options;
using Force.DeepCloner;
using Microsoft.Extensions.DependencyInjection;

namespace Citolab.Repository.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddInMemoryRepository(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddMemoryCache();
            services.AddLogging();
            services.AddSingleton(new InMemoryDatabaseOptions());
            services.AddScoped<IRepositoryFactory, NoActionFactory>();
            return services;
        }

        /// <summary>
        ///     Deep clone using JSON serialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toClone"></param>
        /// <returns></returns>
        public static T Clone<T>(this T toClone) where T : class =>
            toClone.DeepClone();
    }
}

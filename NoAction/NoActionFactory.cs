using System;
using System.Collections.Concurrent;
using Citolab.Repository.Decorators;
using Citolab.Repository.Model;
using Citolab.Repository.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Citolab.Repository.NoAction
{
    /// <summary>
    ///     Factory doesnt do anything with storage. Can be decorated with a memory repository to do the 'storage'
    /// </summary>
    public class NoActionFactory : RepositoryFactoryBase
    {
        private readonly ILoggedInUserProvider _loggedInUserProvider;
        private readonly ILogger _logger;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="memoryCache"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="options"></param>
        /// <param name="loggedInUserProvider"></param>
        public NoActionFactory(IMemoryCache memoryCache, ILoggerFactory loggerFactory, IRepositoryOptions options, ILoggedInUserProvider loggedInUserProvider)
            : base(memoryCache, loggerFactory, options)
        {
            _loggedInUserProvider = loggedInUserProvider;
            Repositories = new ConcurrentDictionary<Type, object>();
            _logger = loggerFactory.CreateLogger(GetType());
        }

        /// <summary>
        ///     Repositories
        /// </summary>
        public ConcurrentDictionary<Type, object> Repositories { get; set; }

        /// <inheritdoc />
        public override IRepository<T> GetRepository<T>()
        {
            if (Repositories.TryGetValue(typeof(T), out var repository))
            {
                return (IRepository<T>)repository;
            }
            var noActionRepository = new FlagAsDeletedDecorator<T>(MemoryCache,
                new FillDefaultValueDecorator<T>(MemoryCache,
                    new CacheDecorator<T>(MemoryCache, true,
                        new NoActionRepository<T>()), _loggedInUserProvider));
            if (LogTime)
            {
                var timeLoggedNoActionRepository =
                    new LogTimeDecorator<T>(MemoryCache, noActionRepository, _logger);
                Repositories.TryAdd(typeof(T), timeLoggedNoActionRepository);
            }
            else
            {
                Repositories.TryAdd(typeof(T), noActionRepository);
            }
            Repositories.TryAdd(typeof(T), noActionRepository);
            return noActionRepository;
        }
    }
}
﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Citolab.Repository.Decorators
{
    public class LogTimeDecorator<T> : RepositoryDecoratorBase<T> where T : Model, new()
    {
        private readonly ILogger _logger;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public LogTimeDecorator(IMemoryCache memoryCache, IRepository<T> decoree, ILogger logger) : base(memoryCache, decoree)
        {
            _logger = logger;
        }

        public override Task<T> AddAsync(T document)
        {
            _stopwatch.Restart();
            var result = base.AddAsync(document).Result;
            _stopwatch.Stop();
            _logger.LogDebug($"AddAsync(document)|{_stopwatch.ElapsedMilliseconds}");
            return Task.FromResult(result);
        }

        public override Task<T> GetAsync(Guid id)
        {
            _stopwatch.Restart();
            var result = base.GetAsync(id).Result;
            _stopwatch.Stop();
            _logger.LogDebug($"GetAsync(id)|{_stopwatch.ElapsedMilliseconds}");
            return Task.FromResult(result);
        }

        public override Task<T> FirstOrDefaultAsync()
        {
            _stopwatch.Restart();
            var result = base.FirstOrDefaultAsync().Result;
            _stopwatch.Stop();
            _logger.LogDebug($"FirstOrDefaultAsync()|{_stopwatch.ElapsedMilliseconds}");
            return Task.FromResult(result);
        }

        public override Task<bool> UpdateAsync(T document)
        {
            _stopwatch.Restart();
            var result = base.UpdateAsync(document).Result;
            _stopwatch.Stop();
            _logger.LogDebug($"UpdateAsync(document)|{_stopwatch.ElapsedMilliseconds}");
            return Task.FromResult(result);
        }
    }
}
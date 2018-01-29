using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Citolab.Repository.Options
{
    public class InMemoryDatabaseOptions : IInMemoryDatabaseOptions
    {
        public bool FlagDelete { get; set; } = true;
        public bool TimeLoggingEnabled { get; set; }
    }
}


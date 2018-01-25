using System;
using System.Collections.Generic;
using System.Text;

namespace Citolab.Repository.Options
{
    public interface IMongoEventStoreOptions: IEventStoreOptions
    {
        string DatabaseName { get; set; }

        string ConnectionString { get; set; }
    }
}

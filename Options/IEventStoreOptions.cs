using System;
using System.Collections.Generic;
using System.Text;

namespace Citolab.Repository.Options
{
    public interface IEventStoreOptions
    {
        string CollectionName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Citolab.Repository.Options
{
    public class InMemoryEventStoreOptions: IInMemoryEventStoreOptions
    {
        public InMemoryEventStoreOptions(string collectionName)
        {
            CollectionName = collectionName;
        }
        public string CollectionName { get; set; }
    }
}

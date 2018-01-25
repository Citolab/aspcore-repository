# Citolab Repository for ASP.NET Core

This repository that can be configured to use MongoDB or an InMemory database.
There is also experimental support for SQL Server.

## IRepository Usage

### In Memory database
```C#
var services = new ServiceCollection();
services.AddRepository(new InMemoryDatabaseOptions());

```
### MongoDB
```C#
var services = new ServiceCollection();
services.AddRepository(new MongoDatabaseOptions("MyDatabase", Configuration.GetConnectionString("MongoDB")));

```

### Sql Server

Sql server support is very experimental and will contain bugs. 
```C#
var services = new ServiceCollection();
services.AddRepository(new SqlServerDatabaseOptions("MyDatabase", Configuration.GetConnectionString("SqlServer")));

```

### Model

An entity that must be stored in the database should inherit from ObjectBase.


### API

In the example below an example controller that uses all methods:

```C#
public class UserController : Controller
{
    private readonly IRepositoryFactory _repositoryFactory;

    public UserController(IRepositoryFactory repositoryFactory) => 
        _repositoryFactory = repositoryFactory;
        
    [HttpGet("{id}")]
    public Task<User> Get(Guid id) => 
        _repositoryFactory.GetRepository<User>().GetAsync(id);

    [HttpGet]
    public Task<IEnumerable<User>> Get() => 
        Task.Run(() => _repositoryFactory.GetRepository<User>().AsQueryable().AsEnumerable());

    [HttpPost]
    public Task<User> Post([FromBody] User user) =>
        _repositoryFactory.GetRepository<User>().AddAsync(user);

    [HttpPut]
    public Task<bool> ChangeName([FromBody] User user) =>
        _repositoryFactory.GetRepository<User>().UpdateAsync(user);

    [HttpDelete("{id}")]
    public Task<bool> Delete(Guid id) =>
        _repositoryFactory.GetRepository<User>().DeleteAsync(id);
}
```

### Default Values
Created and LastModified are filled by default (existing values will be overwritten). CreatedByUserId and LastModifiedByUserId are provided by a registered instance of ILoggedInUserProvider. By default this won't fill a userId. You can write your own instance of ILoggedInUserProvider and register it **before** calling .AddRepository.

If you want to set the Created, LastModified, CreatedByUserId and LastModifiedByUserId then you can create an instance of the OverrideDefaultValues class.
```C#
using (new OverrideDefaultValues()) {
    // your code here
}
```

## IEventStore Usage

IEvent store is a minimalistic event store that has two methods: Save(event) and Get(size, page)

### In Memory database
```C#
services.AddEventStore<Event>(new InMemoryEventStoreOptions("MyEvents"));
```
### MongoDB

```C#
services.AddEventStore<Event>(new MongoEventStoreOptions("MyDatabase", Configuration.GetConnectionString("MongoDB"), "MyEvents"));
```

### API

In the example below a service that call the two methods. The Get method will return rows from old to new.

```C#
public class EventStoreService
{
    private readonly IEventStore<Event> _eventStore;

    public EventStoreService(IEventStore<Event> eventStore) => _eventStore = eventStore;

    public void Save(Event e) => _eventStore.Save(e);

    public IEnumerable<Event> GetAll()
    {
        var all = new List<Event>();
        for (var page = 1; page < int.MaxValue; page++)
        {
            var rows = _eventStore.Get(50, page).ToList();
            all.AddRange(rows);
            if (!rows.Any()) break;
        }
        return all;
    }
}
```
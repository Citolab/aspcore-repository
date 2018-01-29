# Citolab Repository for ASP.NET Core

This is a repository library that can be configured to use MongoDB or an in-memory database (using MemoryCache).
There is also experimental support for SQL Server.

Packages can be installed using NuGet:
- Install-Package Citolab.Repository (in-memory)
- Install-Package Citolab.Repository.Mongo (MongoDB)
- Install-Package Citolab.Repository.SqlServer (SQL server)

## IRepository Usage

### In-memory database
```C#
var services = new ServiceCollection();
services.AddInMemoryRepository();

```
### MongoDB
```C#
var services = new ServiceCollection();
services.AddMongoRepository("MyDatabase", Configuration.GetConnectionString("MongoDB"));

```

### Sql Server

Sql server support is very experimental and will contain bugs. 
```C#
var services = new ServiceCollection();
services.AddSqlServerRepository("MyDatabase", Configuration.GetConnectionString("SqlServer"));

```

### Model

An entity that must be stored in the database should inherit from Model.


### API

In the example below a Web API controller that uses all methods:

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
### Caching

Caching can be added using Mongo or SQL Server as database too. Adding the caching attribute above your model classes; these entities will be kept in the MemoryCache. The cache will be updated when entities are changed.

```C#
[Cache(300)]
public class User : Model
{
	//properties
}
```
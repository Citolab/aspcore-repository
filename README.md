# Citolab Repository for ASP.NET Core

This repository that can be configured to use MongoDB or an InMemory database. The last one can be very handy in UnitTests.
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

Sql server support is very experimental. 
```C#
var services = new ServiceCollection();
services.AddRepository(new SqlServerDatabaseOptions("MyDatabase", Configuration.GetConnectionString("SqlServer")));

```

### Model

Each class that should be an entity in the database should inherit from ObjectBase.

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


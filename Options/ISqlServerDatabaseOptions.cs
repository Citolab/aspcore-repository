namespace Citolab.Repository.Options
{
    public interface ISqlServerDatabaseOptions: IRepositoryOptions
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
    }
}
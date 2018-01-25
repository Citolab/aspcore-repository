namespace Citolab.Repository.Options
{
    public interface IMongoDatabaseOptions: IRepositoryOptions
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
    }
}
using Citolab.Repository.Options;

namespace Citolab.Repository.Mongo.Options
{
    public interface IMongoDatabaseOptions: IRepositoryOptions
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
    }
}
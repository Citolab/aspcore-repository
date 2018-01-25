using Citolab.Repository.Model;

namespace Citolab.Repository
{
    /// <summary>
    ///     Interface for repository factory
    /// </summary>
    public interface IRepositoryFactory
    {
        /// <summary>
        ///     Get the repository
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> GetRepository<T>() where T : ObjectBase, new();
    }
}
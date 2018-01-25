using System;
using System.Threading.Tasks;
using Citolab.Repository.Helpers;
using Citolab.Repository.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Citolab.Repository.Decorators
{
    /// <summary>
    ///     Used to fill default values like, created, createdby, modified, modifiedby, id etc..
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FillDefaultValueDecorator<T> : RepositoryDecoratorBase<T> where T : ObjectBase, new()
    {
        private readonly ILoggedInUserProvider _loggedInUserProvider;

        /// <inheritdoc />
        public FillDefaultValueDecorator(IMemoryCache memoryCache, IRepository<T> decoree, 
            ILoggedInUserProvider loggedInUserProvider)
            : base(memoryCache, decoree)
        {
            _loggedInUserProvider = loggedInUserProvider;
        }

        /// <inheritdoc />
        public override async Task<T> AddAsync(T document)
        {
            var userId = document.CreatedByUserId == Guid.Empty
                ? _loggedInUserProvider?.GetUserId()
                : document.CreatedByUserId;
            if (document.Id == default(Guid)) document.Id = Guid.NewGuid();
            if (userId.HasValue)
            {
                document.CreatedByUserId =
                    !OverrideDefaultValues.FillDefaulValues && document.CreatedByUserId != default(Guid)
                        ? document.CreatedByUserId
                        : userId.Value;
                document.LastModifiedByUserId =
                    !OverrideDefaultValues.FillDefaulValues && document.LastModifiedByUserId != default(Guid)
                        ? document.LastModifiedByUserId
                        : userId.Value;
            }
            var dateNow = !OverrideDefaultValues.FillDefaulValues && document.Created != default(DateTime)
                ? document.Created
                : DateTime.UtcNow;
            document.Created = dateNow;
            document.LastModified = dateNow;
            return await base.AddAsync(document);
        }


        /// <inheritdoc />
        public override async Task<bool> UpdateAsync(T document)
        {
            var userId = _loggedInUserProvider?.GetUserId();
            if (!userId.HasValue || userId.Value == Guid.Empty)
            {
                userId = _loggedInUserProvider?.GetUserId();
            }
            if (userId.HasValue)
                document.LastModifiedByUserId =
                    !OverrideDefaultValues.FillDefaulValues && document.LastModifiedByUserId != default(Guid)
                        ? document.LastModifiedByUserId
                        : userId.Value;

            var dateNow = !OverrideDefaultValues.FillDefaulValues && document.LastModified != default(DateTime)
                ? document.LastModified
                : DateTime.UtcNow;
            document.LastModified = dateNow;
            return await base.UpdateAsync(document);
        }
    }
}
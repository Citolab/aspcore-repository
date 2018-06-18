using System;

namespace Citolab.Repository
{
    /// <summary>
    ///     Get logged in user interface.
    /// </summary>
    public interface ILoggedInUserProvider
    {
        object UserObject { get; set; }
        Guid? GetUserId();
    }
}
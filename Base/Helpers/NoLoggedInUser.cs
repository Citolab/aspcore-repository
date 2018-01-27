using System;
using System.Collections.Generic;
using System.Text;

namespace Citolab.Repository.Helpers
{
    public class NoLoggedInUser : ILoggedInUserProvider
    {
        public Guid? GetUserId() => null;
    }
}

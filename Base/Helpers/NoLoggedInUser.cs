using System;
using System.Collections.Generic;
using System.Text;

namespace Citolab.Repository.Helpers
{
    public class NoLoggedInUser : ILoggedInUserProvider
    {
        public object UserObject { get; set; }
        public Guid? GetUserId() => null;
    }
}

using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MenuSystemDemo.Identity
{
    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
        public ApplicationUser User { get; set; }
    }
}
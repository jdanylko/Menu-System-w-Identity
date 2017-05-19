using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MenuSystemDemo.Identity
{
    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
        public ApplicationUser User { get; set; }

        public override int UserId { get; set; }
    }
}
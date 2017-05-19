using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MenuSystemDemo.Identity
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public override int UserId { get; set; }
        public override int RoleId { get; set; }
        
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
    }
}
using System.Collections.Generic;
using MenuSystemDemo.GeneratedClasses;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MenuSystemDemo.Identity
{
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>, IRole<int>
    {
        public ApplicationRole() { }
        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
            MenuItems = new HashSet<ApplicationRoleMenu>();

        }

        public ICollection<ApplicationRoleMenu> MenuItems { get; set; }

    }
}
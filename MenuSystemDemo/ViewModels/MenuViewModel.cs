using System.Collections.Generic;
using MenuSystemDemo.GeneratedClasses;
using MenuSystemDemo.Identity;

namespace MenuSystemDemo.ViewModels
{
    public class MenuViewModel
    {
        public ICollection<MenuItem> MenuItems { get; set; }

        public ApplicationUser User { get; set; }
        public IEnumerable<ApplicationRole> Roles { get; set; }
        public ICollection<MenuPermission> Permissions { get; set; }
    }
}
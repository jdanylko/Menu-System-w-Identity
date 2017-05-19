using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using MenuSystemDemo.GeneratedClasses;

namespace MenuSystemDemo.Identity
{
    /// <summary>
    /// To create your own permission, add an entry to the Permissions table
    /// and call GetMenuByUser() by passing in your own permission.
    /// </summary>
    public class MenuManager
    {
        private readonly ApplicationDbContext _context;

        public MenuManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<MenuItem> GetMenuByUser(ApplicationUser user, 
            System.Func<MenuPermission, bool> filterFunc = null)
        {
            if (user == null)
            {
                return new Collection<MenuItem>();
            }

            // Get Ids.
            var roleIds = user.Roles.Select(role => role.RoleId).ToList();

            // Enable eager-loading to retrieve our permissions as well.
            var items = _context.MenuItems
                .Include(menu => menu.Roles.Select(role => role.Permissions))
                .Where(e => e.Roles.Any(roleMenu => roleIds.Contains(roleMenu.RoleId)));

            ICollection<MenuItem> records;
            if (filterFunc == null)
            {
                records = items.Where(e => e.Roles.Any(f => f.Permissions.Any())).ToList();
            }
            else
            {
                records = items.Where(e => e.Roles.Any(f => f.Permissions.Any(filterFunc))).ToList();
            }

            return records;
        }

        public ICollection<MenuItem> GetAllByUser(ApplicationUser user)
        {
            return GetMenuByUser(user);
        }



        public ICollection<MenuItem> GetViewableMenuItems(ApplicationUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "View");
        }

        public ICollection<MenuItem> GetCreateMenuItems(ApplicationUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "Create");
        }

        public ICollection<MenuItem> GetDeleteMenuItems(ApplicationUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "Delete");
        }

        public ICollection<MenuItem> GetUpdateMenuItems(ApplicationUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "Update");
        }

        public ICollection<MenuItem> GetUploadMenuItems(ApplicationUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "Upload");
        }

        public ICollection<MenuItem> GetPublishMenuItems(ApplicationUser user)
        {
            return GetMenuByUser(user, menuPermission => menuPermission.Permission.Name == "Publish");
        }

    }
}
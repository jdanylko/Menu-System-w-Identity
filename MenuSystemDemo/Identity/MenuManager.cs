using System;
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

        public ICollection<MenuPermission> GetMenuPermissionsByUser(ApplicationUser user)
        {
            if (user == null)
            {
                return new Collection<MenuPermission>();
            }

            return _context.Roles
                .Include(role => role.MenuItems.Select(menu => menu.Permissions))
                .Where(role => role.Users.Any(tableUser => tableUser.UserId == user.Id))
                .SelectMany(role => role.MenuItems.SelectMany(roleMenu => roleMenu.Permissions))
                .ToList();
        }


        public ICollection<MenuItem> GetMenuByUser(ApplicationUser user,
            Func<MenuPermission, bool> filterFunc = null)
        {
            var items = GetMenuPermissionsByUser(user);

            var records = filterFunc == null 
                ? items.ToList() 
                : items.Where(filterFunc).ToList();

            return records
                .GroupBy(menuPermission => menuPermission.RoleMenu.MenuItem)
                .Select(grouping => grouping.Key)
                .ToList();
        }

        public ICollection<MenuItem> GetAllByUser(ApplicationUser user)
        {
            return GetMenuByUser(user);
        }

        public IEnumerable<MenuItem> GetViewableMenuItems(ApplicationUser user)
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
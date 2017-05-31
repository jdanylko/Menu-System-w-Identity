using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MenuSystemDemo.GeneratedClasses;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace MenuSystemDemo.Identity
{
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEf(context);
            base.Seed(context);
        }

        public static void InitializeIdentityForEf(ApplicationDbContext context)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(context));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));

            CreatePermissions(context);

            var menuItems = CreateMenu(context);

            var roles = CreateRoles(roleManager);

            // User
            // +--Roles (Developer, Administrator, etc.)
            //   +--MenuItem (each MenuItem a single user can use)
            //     +--Permissions (what can the user do per menu item...create, update, delete, etc.)

            // Administrator User
            var adminUser = CreateAdministrator(context, userManager);
            
            // Assign the role to the user.
            var administrator = roles.FirstOrDefault(role => role == "Administrator");
            userManager.AddToRoles(adminUser.Id, administrator);
            context.SaveChanges();


            // Developer User
            var devUser = CreateDeveloperUser(context, userManager);
            
            // Setup the developers.
            var developer = roles.FirstOrDefault(e => e == "Developer");
            userManager.AddToRoles(devUser.Id, developer);
            context.SaveChanges();



            // Developer Role
            var developerRole = roleManager.FindByName("Developer");
            // Add all of the menuItems responsible for a developer
            // (Everything except for security).
            var menuItemsInDeveloper = menuItems
                .Select(e => new ApplicationRoleMenu
                {
                    RoleId = developerRole.Id,
                    MenuId = e.Id
                }).ToList();

            foreach (var roleMenu in menuItemsInDeveloper)
            {
                developerRole.MenuItems.Add(roleMenu);
                context.SaveChanges();

                // If it's not a security part of the system, 
                //   allow the developer to access everything.
                if (roleMenu.MenuItem.Title == "Security") continue;

                // Adding the security
                for (int i = 0; i < 6; i++)
                {
                    roleMenu.Permissions.Add(new MenuPermission { RoleMenuId = roleMenu.Id, PermissionId = i+1 });
                }
            }
            context.SaveChanges();



            
            // Administrator Role.
            var administratorRole = roleManager.FindByName("Administrator");
            var menuItemsInAdministrator = menuItems
                .Select(e => new ApplicationRoleMenu
                {
                    RoleId = administratorRole.Id,
                    MenuId = e.Id
                })
                .ToList();

            foreach (var roleMenu in menuItemsInAdministrator)
            {
                administratorRole.MenuItems.Add(roleMenu);
                context.SaveChanges();

                // Add ALL permissions for an Administrator.
                for (int i = 0; i < 6; i++)
                {
                    roleMenu.Permissions.Add(new MenuPermission { RoleMenuId = roleMenu.Id, PermissionId = i + 1 });
                }
            }
            context.SaveChanges();


            // Let's get our updated Ids
            var menuManager = new MenuManager(context);

            // DevPerms - No, it's not a new developer hairstyle. :-p
            var devPerms = menuManager.GetMenuPermissionsByUser(devUser);

            // Add our claims to each role.

            // Developer
            foreach (var menuPermission in devPerms)
            {
                devUser.Claims.Add(new ApplicationUserClaim
                {
                    UserId = devUser.Id,
                    ClaimType = menuPermission.RoleMenu.MenuId.ToString(),
                    ClaimValue = menuPermission.Permission.Name
                });
            }
            context.SaveChanges();

            // Administrator
            var adminPerms = menuManager.GetMenuPermissionsByUser(adminUser);

            foreach (var menuPermission in adminPerms)
            {
                adminUser.Claims.Add(new ApplicationUserClaim
                {
                    UserId = adminUser.Id,
                    ClaimType = menuPermission.RoleMenu.MenuId.ToString(),
                    ClaimValue = menuPermission.Permission.Name
                });
            }
            context.SaveChanges();

        }

        private static ApplicationUser CreateAdministrator(ApplicationDbContext db, ApplicationUserManager userManager)
        {
            // Create the administrator
            var user = userManager.FindByEmail("bob@gmail.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = "bob@gmail.com",
                    UserName = "bob",
                    EmailConfirmed = true
                };
                userManager.Create(user, "Password123!");
            }
            db.SaveChanges();

            return user;
        }

        private static ApplicationUser CreateDeveloperUser(ApplicationDbContext db, ApplicationUserManager userManager)
        {
            // Create the administrator
            var user = userManager.FindByEmail("frank@gmail.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = "frank@gmail.com",
                    UserName = "frank",
                    EmailConfirmed = true
                };
                userManager.Create(user, "Password456!");
            }
            db.SaveChanges();

            return user;
        }

        private static string[] CreateRoles(ApplicationRoleManager roleManager)
        {
            string[] roles = {"Administrator", "Publisher", "Editor", "Developer", "Designer", "Copywriter"};
            foreach (string role in roles)
            {
                if (roleManager.RoleExists(role)) continue;
                var roleResult = roleManager.Create(new ApplicationRole(role));
            }

            return roles;
        }

        private static List<MenuItem> CreateMenu(ApplicationDbContext db)
        {
            // Seed the MenuItems
            var menuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = 1,
                    Title = "Setup",
                    Description = "Setup the system",
                    ParentId = null,
                    Icon = "wrench",
                    Url = null
                },
                new MenuItem
                {
                    Id = 2,
                    Title = "Users",
                    Description = "Manage Users",
                    ParentId = 1,
                    Icon = "user",
                    Url = "/setup/users"
                },
                new MenuItem
                {
                    Id = 3,
                    Title = "Security",
                    Description = "Set Permissions for System",
                    ParentId = 1,
                    Icon = "lock",
                    Url = "/setup/security"
                },
                new MenuItem
                {
                    Id = 4,
                    Title = "Menu Management",
                    Description = "Manage the Menus for the System",
                    ParentId = 1,
                    Icon = "list",
                    Url = "/Setup/Menu"
                }
            };

            foreach (var menuItem in menuItems)
            {
                db.MenuItems.Add(menuItem);
            }
            db.SaveChanges();
            return menuItems;
        }

        private static void CreatePermissions(ApplicationDbContext db)
        {
            // Create the permissions
            var permissions = new List<Permission>
            {
                new Permission {Id = 1, Name = "Create"},
                new Permission {Id = 2, Name = "View"},
                new Permission {Id = 3, Name = "Update"},
                new Permission {Id = 4, Name = "Delete"},
                new Permission {Id = 5, Name = "Publish"},
                new Permission {Id = 6, Name = "Upload"}
            };
            foreach (var permission in permissions)
            {
                db.Permissions.Add(permission);
            }
            db.SaveChanges();
        }
    }
}
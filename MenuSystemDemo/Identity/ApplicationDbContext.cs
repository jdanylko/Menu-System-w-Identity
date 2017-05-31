using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using MenuSystemDemo.GeneratedClasses;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MenuSystemDemo.Identity
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, int,
            ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext(): base("MenuDatabaseEntities")
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder error");
            }

            // Needed to ensure subclasses share the same table
            var user = modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));

            // CONSIDER: u.Email is Required if set on options?
            user.Property(u => u.Email).HasMaxLength(256);

            modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { r.UserId, r.RoleId }).ToTable("AspNetUserRoles");

            modelBuilder.Entity<ApplicationUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("AspNetUserLogins");

            var role = modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");
            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);


            /////////////////////////////////////////
            // All of this is standard but using our entity types (ApplicationXxxxxx) instead of IdentityXxxxxx
            // Ref: https://aspnetidentity.codeplex.com/SourceControl/latest#src/Microsoft.AspNet.Identity.EntityFramework/IdentityDbContext.cs
            /////////////////////////////////////////

            /* Role-Menu Definitions */

            modelBuilder.Entity<MenuItem>().ToTable("AspNetMenu");
            modelBuilder.Entity<MenuItem>()
                .HasMany(e => e.Children)
                .WithOptional(e => e.ParentItem)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<MenuItem>()
                .HasMany(e => e.Roles)
                .WithRequired(e => e.MenuItem)
                .HasForeignKey(e => e.MenuId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationRoleMenu>().ToTable("AspNetRoleMenu");

            modelBuilder.Entity<ApplicationRoleMenu>()
                .HasMany(e => e.Permissions)
                .WithRequired(e => e.RoleMenu)
                .HasForeignKey(e => e.RoleMenuId)
                .WillCascadeOnDelete(false);

        }

        public IDbSet<MenuItem> MenuItems { get; set; }

        public IDbSet<Permission> Permissions { get; set; }
    }
}
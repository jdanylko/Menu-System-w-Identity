using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MenuSystemDemo.GeneratedClasses;

namespace MenuSystemDemo.Identity
{
    public class ApplicationRoleMenu
    {
        public ApplicationRoleMenu()
        {
            Permissions = new HashSet<MenuPermission>();
        }

        [Key, Column(Order=1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        [Column(Order = 2)]
        public virtual int RoleId { get; set; }

        [Column(Order = 3)]
        public virtual int MenuId { get; set; }

        public virtual ApplicationRole Role { get; set; }
        public virtual MenuItem MenuItem { get; set; }
        public ICollection<MenuPermission> Permissions { get; set; }

    }
}
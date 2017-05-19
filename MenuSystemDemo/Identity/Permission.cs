using System.ComponentModel.DataAnnotations;

namespace MenuSystemDemo.Identity
{
    public class Permission
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
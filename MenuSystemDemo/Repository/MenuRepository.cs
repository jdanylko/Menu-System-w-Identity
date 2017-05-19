using System.Data.Entity;
using MenuDemo.Repository;
using MenuSystemDemo.GeneratedClasses;
using MenuSystemDemo.Identity;

namespace MenuSystemDemo.Repository
{
    public class MenuRepository : Repository<MenuItem>
    {
        public MenuRepository(DbContext objectContext)
            : base(objectContext)
        {
        }
        public MenuRepository() : this(new ApplicationDbContext())
        {
        }
    }
}
using Microsoft.EntityFrameworkCore;
using User_Service.Models;

namespace User_Service.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        // Entities        
        public DbSet<User> Users { get; set; }
    }
}

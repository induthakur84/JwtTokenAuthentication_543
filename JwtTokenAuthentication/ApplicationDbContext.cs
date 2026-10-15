using Microsoft.EntityFrameworkCore;

namespace JwtTokenAuthentication
{

    //DbContext class
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }



        public DbSet<User> Users { get; set; }  

        public DbSet<Product> Products { get; set; }
    }
}

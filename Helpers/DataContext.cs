using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShoppingAssistantServer.Entities;

namespace ShoppingAssistantServer.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            
            options.UseSqlServer(Configuration.GetConnectionString("ShoppingAssistantServerDb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Productstore>().HasKey(ps => new { ps.Id_product, ps.Id_store});
            modelBuilder.Entity<Storeschedules>().HasKey(ss => new { ss.Id_store });
            modelBuilder.Entity<Admin>().HasKey(adm => new { adm.Id_user });
            modelBuilder.Entity<StoreAdmin>().HasKey(sa => new { sa.Id_admin, sa.Id_store });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Storeschedules> Storeschedules { get; set; }
        public DbSet<Stores> Stores{ get; set; }
        public DbSet<Products> Products{ get; set; }
        public DbSet<Productstore> Productstore { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<StoreAdmin> StoreAdmin { get; set; }

    }
}
using EVRP_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EVRP_Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Driver> Drivers => Set<Driver>();
        public DbSet<Node> Nodes => Set<Node>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(x => x.IsDelivered);

            modelBuilder.Entity<Customer>()
                .HasIndex(x => x.Name);

            modelBuilder.Entity<Driver>()
                .HasIndex(x => x.IsActive);

            modelBuilder.Entity<Node>()
                .HasIndex(x => x.NodeType);
        }
    }
}

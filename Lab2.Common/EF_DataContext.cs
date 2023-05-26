using Lab2.DAL;
using Microsoft.EntityFrameworkCore;

namespace Lab2
{
    public class EF_DataContext : DbContext
    {
        public EF_DataContext(DbContextOptions<EF_DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.UseSerialColumns();
        }

        public DbSet<TbProduct> Products { get; set; }

        public DbSet<TbOrder> Orders { get; set; }

        public DbSet<TbUser> Users { get; set; }
    }
}

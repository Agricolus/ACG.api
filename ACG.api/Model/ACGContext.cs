using ACG.api.Model;
using Microsoft.EntityFrameworkCore;

namespace ACG
{

    public class PostgresContext : ACGContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
               => optionsBuilder.UseNpgsql("Host=192.168.1.181;Database=ACG;Username=postgres;Password=postgres");
    }

    public class ACGContext : DbContext
    {
        public ACGContext() { }
        public ACGContext(DbContextOptions<ACGContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp")
                               .Entity<Machine>()
                               .Property(e => e.Id)
                               .HasDefaultValueSql("uuid_generate_v4()");
            // .. and invoke "BuildIndexesFromAnnotations"!
            // modelBuilder.BuildIndexesFromAnnotations();
        }

        public DbSet<Machine> Machines { get; set; }
    }
}

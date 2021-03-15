using ACG.api.Model;
using Microsoft.EntityFrameworkCore;

namespace ACG
{

    public class PostgresContext : ACGContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
               => optionsBuilder.UseNpgsql("Host=192.168.1.181;Port=5433;Database=ACG;Username=postgres;Password=postgres", x => x.UseNetTopologySuite());
    }

    public class ACGContext : DbContext
    {
        public ACGContext() { }
        public ACGContext(DbContextOptions<ACGContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("postgis");
            var uuidOsspExtension = modelBuilder.HasPostgresExtension("uuid-ossp");
            uuidOsspExtension.Entity<Machine>()
                               .Property(e => e.Id)
                               .HasDefaultValueSql("uuid_generate_v4()");
            uuidOsspExtension.Entity<MachineHistory>()
                               .Property(e => e.Id)
                               .HasDefaultValueSql("uuid_generate_v4()");
            uuidOsspExtension.Entity<Field>()
                               .Property(e => e.Id)
                               .HasDefaultValueSql("uuid_generate_v4()");
        }

        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineHistory> MachinesHistory { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}

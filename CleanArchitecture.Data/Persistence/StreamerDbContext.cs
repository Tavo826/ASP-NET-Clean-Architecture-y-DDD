using CleanArchitecture.Domain;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public class StreamerDbContext : DbContext
    {
        public StreamerDbContext(DbContextOptions<StreamerDbContext> options) : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //usando la seguridad de SQL
        //    // optionsBuilder.UseSqlServer("Data Source=PSOFKA0699;Initial Catalog=Streamer;user id=sa;password=y2Ndo1bKRF; TrustServerCertificate=True");
        //    //usando la seguridad de windows
        //    optionsBuilder.UseSqlServer("Data Source=PSOFKA0699;Initial Catalog=Streamer;Integrated Security=True; TrustServerCertificate=True")
        //        .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information)
        //        .EnableSensitiveDataLogging();
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //Seteando los valores de auditoria de la entidad BaseDomainModel
            foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "userSystem";
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "userSystem";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación opcional de unos a muchos en fluetApi ya que está configurado por EF
            // Se usa cuando se tiene una clave foránea con un nombre diferente a {Modelo}Id
            modelBuilder.Entity<Streamer>()
                .HasMany(m => m.Videos)
                .WithOne(m => m.Streamer)
                .HasForeignKey(m => m.StreamerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Creando relación de VideoActor con Video y Actor
            modelBuilder.Entity<Video>()
                .HasMany(p => p.Actores)
                .WithMany(test => test.Videos)
                .UsingEntity<VideoActor>(
                    pt => pt.HasKey(e => new { e.ActorId, e.VideoId })
                );
        }
        public DbSet<Streamer> Streamers { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Director> Directores { get; set; }
    }
}

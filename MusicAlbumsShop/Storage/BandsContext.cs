using Microsoft.EntityFrameworkCore;
using MusicAlbumsShop.Models;

namespace MusicAlbumsShop.Storage
{
    public class BandsContext : DbContext
    {
        public DbSet<Band> Bands { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Album> Albums { get; set; }

        public BandsContext(DbContextOptions<BandsContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(64);

            modelBuilder.Entity<Band>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Band>().Property(x => x.Origin).IsRequired();
            modelBuilder.Entity<Band>().Property(x => x.YearsActive).IsRequired();
            modelBuilder.Entity<Band>().Property(x => x.GenreId).IsRequired();
            modelBuilder.Entity<Band>().HasOne(x => x.Genre).WithMany(x => x.Bands).HasForeignKey(x => x.GenreId);
            
            modelBuilder.Entity<Album>().Property(x => x.Title).IsRequired();
            modelBuilder.Entity<Album>().Property(x => x.ReleaseDate).IsRequired();
            modelBuilder.Entity<Album>().Property(x => x.BandId).IsRequired();
            modelBuilder.Entity<Album>().HasOne(x => x.Band).WithMany().HasForeignKey(x => x.BandId);

        }

    }    
}

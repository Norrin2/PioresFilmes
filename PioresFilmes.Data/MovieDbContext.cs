using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PioresFilmes.Domain;

namespace PioresFilmes.Data
{
    public class MovieDbContext: DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Producer> Producers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=file::memory:?cache=shared")
                .LogTo(Console.WriteLine, LogLevel.Information); // TODO - REMOVER
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                        .HasKey(m => m.Id);

            modelBuilder.Entity<Producer>()
                        .HasKey(p => p.Id);

            modelBuilder.Entity<Movie>()
                        .HasMany(m => m.Producers)
                        .WithMany();
        }
    }
}

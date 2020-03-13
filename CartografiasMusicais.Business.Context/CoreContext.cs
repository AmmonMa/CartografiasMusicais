using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CartografiasMusicais.Business.Context
{
    public class CoreContext : IdentityDbContext<User>
    {
        public CoreContext()
        {
        }

        public CoreContext(DbContextOptions<CoreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cidade>()
                        .HasMany(x => x.Musicas)
                        .WithOne(y => y.Cidade)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Cidade>()
                        .HasMany(x => x.Corpos)
                        .WithOne(y => y.Cidade)
                        .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Cidade>()
                        .HasMany(x => x.Grupos)
                        .WithOne(y => y.Cidade)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Cidade>()
                        .HasMany(x => x.Narrativas)
                        .WithOne(y => y.Cidade)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Narrativa>()
                        .HasMany(x => x.Musicos)
                        .WithOne(y => y.Narrativa)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Narrativa>()
                        .HasMany(x => x.Frequentadores)
                        .WithOne(y => y.Narrativa)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Narrativa>()
                        .HasMany(x => x.Vozes)
                        .WithOne(y => y.Narrativa)
                        .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Musica> Musicas { get; set; }
        public DbSet<Corpo> Corpos { get; set; }
        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Espaco> Espacos { get; set; }
        public DbSet<Narrativa> Narrativas { get; set; }
        public DbSet<Musico> Musicos { get; set; }
        public DbSet<Frequentador> Frequentadores { get; set; }
        public DbSet<Voz> Vozes { get; set; }

    }
}

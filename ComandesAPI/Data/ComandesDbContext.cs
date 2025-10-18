using Microsoft.EntityFrameworkCore;
using ComandesAPI.Models;

namespace ComandesAPI.Data
{
    public class ComandesDbContext : DbContext
    {
        public ComandesDbContext(DbContextOptions<ComandesDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Usuari> Usuaris { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nom).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcio).HasMaxLength(500);
                entity.Property(e => e.Preu).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Estoc).IsRequired();
                entity.Property(e => e.Categoria).HasMaxLength(20);
                entity.Property(e => e.Actiu).IsRequired().HasDefaultValue(true);
                entity.Property(e => e.DataCreacio).IsRequired();
                entity.Property(e => e.DataModificacio);

                entity.HasIndex(e => e.Nom).HasDatabaseName("IX_Articles_Nom");
                entity.HasIndex(e => e.Categoria).HasDatabaseName("IX_Articles_Categoria");
            });

            // Configuració de l'entitat Usuari
            modelBuilder.Entity<Usuari>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20).HasDefaultValue("User");
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.IsEnabled).IsRequired().HasDefaultValue(true);

                entity.HasIndex(e => e.Username).IsUnique().HasDatabaseName("IX_Usuaris_Username");
                entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("IX_Usuaris_Email");
            });

            // Aplicar dades SEED des d'arxius separats
            modelBuilder.SeedArticles();
            modelBuilder.SeedUsers();
        }
    }
}
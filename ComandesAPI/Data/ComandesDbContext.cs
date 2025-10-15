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

            // Aplicar datos SEED desde archivo separado
            modelBuilder.SeedArticles();
        }
    }
}
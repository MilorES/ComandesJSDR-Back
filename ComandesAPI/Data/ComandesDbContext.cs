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
        public DbSet<Comanda> Comandes { get; set; }
        public DbSet<LiniaComanda> LiniesComanda { get; set; }

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
                entity.Property(e => e.Actiu).IsRequired();
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
                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20).HasDefaultValue("User");
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.IsEnabled).IsRequired().HasDefaultValue(true);

                entity.HasIndex(e => e.Username).IsUnique().HasDatabaseName("IX_Usuaris_Username");
            });

            // Configuració de l'entitat Comanda
            modelBuilder.Entity<Comanda>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeroComanda).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Estat).IsRequired();
                entity.Property(e => e.DataCreacio).IsRequired();
                entity.Property(e => e.DataModificacio);
                entity.Property(e => e.DataAprovacio);
                entity.Property(e => e.DataFinalitzacio);
                entity.Property(e => e.Observacions).HasMaxLength(1000);
                entity.Property(e => e.Total).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.DescomptePercentatge).HasColumnType("decimal(5,2)").IsRequired();
                entity.Property(e => e.ImportDescompte).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.TotalAmbDescompte).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Actiu).IsRequired().HasDefaultValue(true);

                // Relació amb Usuari
                entity.HasOne(e => e.Usuari)
                    .WithMany()
                    .HasForeignKey(e => e.UsuariId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.NumeroComanda).IsUnique().HasDatabaseName("IX_Comandes_NumeroComanda");
                entity.HasIndex(e => e.UsuariId).HasDatabaseName("IX_Comandes_UsuariId");
                entity.HasIndex(e => e.Estat).HasDatabaseName("IX_Comandes_Estat");
                entity.HasIndex(e => e.DataCreacio).HasDatabaseName("IX_Comandes_DataCreacio");
            });

            // Configuració de l'entitat LiniaComanda
            modelBuilder.Entity<LiniaComanda>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NomProducte).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descripcio).HasMaxLength(500);
                entity.Property(e => e.Quantitat).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.PreuUnitari).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.DescomptePercentatge).HasColumnType("decimal(5,2)").IsRequired();
                entity.Property(e => e.Subtotal).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.ImportDescompte).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Total).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.Ordre).IsRequired();
                entity.Property(e => e.DataCreacio).IsRequired();

                // Relació amb Comanda
                entity.HasOne(e => e.Comanda)
                    .WithMany(c => c.Linies)
                    .HasForeignKey(e => e.ComandaId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relació opcional amb Article
                entity.HasOne(e => e.Article)
                    .WithMany()
                    .HasForeignKey(e => e.ArticleId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(e => e.ComandaId).HasDatabaseName("IX_LiniesComanda_ComandaId");
                entity.HasIndex(e => e.ArticleId).HasDatabaseName("IX_LiniesComanda_ArticleId");
            });

            // Aplicar dades SEED des d'arxius separats
            modelBuilder.SeedArticles();
            modelBuilder.SeedUsers();
            modelBuilder.SeedComandes();
        }
    }
}
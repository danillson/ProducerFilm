using Microsoft.EntityFrameworkCore;
using ProducerFilm.Domain.Entities;

namespace ProducerFilm.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<MovieListHistory> MovieListHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações da entidade MovieListHistory
        modelBuilder.Entity<MovieListHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Configurar propriedades privadas
            entity.Property(e => e.Id)
                .HasColumnName("Id");
            
            entity.Property(e => e.Year)
                .HasColumnName("Year")
                .IsRequired();
            
            entity.Property(e => e.Title)
                .HasColumnName("Title")
                .IsRequired()
                .HasMaxLength(300);
            
            entity.Property(e => e.Studios)
                .HasColumnName("Studios")
                .HasMaxLength(200);
            
            entity.Property(e => e.Producers)
                .HasColumnName("Producers")
                .HasMaxLength(300);
            
            entity.Property(e => e.Winner)
                .HasColumnName("Winner")
                .HasMaxLength(10);
            
            entity.Property(e => e.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();
            
            // Índice para melhorar consultas por ano
            entity.HasIndex(e => e.Year)
                .HasDatabaseName("IX_MovieListHistories_Year");
        });
    }
}

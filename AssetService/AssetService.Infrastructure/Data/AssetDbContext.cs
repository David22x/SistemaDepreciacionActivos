using AssetService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetService.Infrastructure.Data;

public class AssetDbContext : DbContext
{
    public AssetDbContext(DbContextOptions<AssetDbContext> options)
        : base(options) { }

    public DbSet<Activo> Activos => Set<Activo>();
    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Categoria
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.VidaUtilMeses).IsRequired();

            // Seed de las 3 categorías
            entity.HasData(
                new Categoria { Id = 1, Nombre = "Tecnología/Electrónicos/Mueblería", VidaUtilMeses = 36 },
                new Categoria { Id = 2, Nombre = "Vehículos", VidaUtilMeses = 60 },
                new Categoria { Id = 3, Nombre = "Edificios/Construcciones", VidaUtilMeses = 240 }
            );
        });

        // Activo
        modelBuilder.Entity<Activo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ValorOriginal)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();
            entity.Property(e => e.FechaAdquisicion).IsRequired();
            entity.Property(e => e.UsuarioId).IsRequired();
            entity.HasIndex(e => e.UsuarioId);

            entity.HasOne(e => e.Categoria)
                  .WithMany(c => c.Activos)
                  .HasForeignKey(e => e.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict); // No borrar categoría con activos
        });
    }
}
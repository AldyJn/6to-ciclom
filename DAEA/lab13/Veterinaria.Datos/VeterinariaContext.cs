using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades;

namespace Veterinaria.Datos;

public class VeterinariaContext : DbContext
{
    public VeterinariaContext(DbContextOptions<VeterinariaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Mascota> Mascotas { get; set; }

    public virtual DbSet<Propietario> Propietarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Microchip, "UX_Mascotas_Microchip")
                .IsUnique()
                .HasFilter("([Microchip] IS NOT NULL)");

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Especie).HasMaxLength(30);
            entity.Property(e => e.Microchip).HasMaxLength(30);
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.Peso).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Raza).HasMaxLength(60);

            entity.HasOne(d => d.Propietario).WithMany(p => p.Mascotas)
                .HasForeignKey(d => d.PropietarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mascotas_Propietarios");
        });

        modelBuilder.Entity<Propietario>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Documento, "UQ_Propietarios_Documento").IsUnique();

            entity.Property(e => e.Documento).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });
    }
}

using Microsoft.EntityFrameworkCore;
using PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Doctor.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Doctor.Infrastructure.Persistence.EFC;

public class DoctorDbContext(DbContextOptions<DoctorDbContext> options) : DbContext(options)
{
    public DbSet<DoctorProfileAggregate> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración directa de DoctorProfileAggregate
        modelBuilder.Entity<DoctorProfileAggregate>(entity =>
        {
            entity.ToTable("Profiles");
            entity.HasKey(p => p.Id);

            // Configuración de Value Objects como Owned Entities
            entity.OwnsOne(p => p.Name, name =>
            {
                name.Property(n => n.FirstName)
                    .HasColumnName("FirstName")
                    .HasMaxLength(100)
                    .IsRequired();
                    
                name.Property(n => n.LastName)
                    .HasColumnName("LastName")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            entity.OwnsOne(p => p.Specialty, specialty =>
            {
                specialty.Property(s => s.Value)
                    .HasColumnName("Specialty")
                    .HasMaxLength(100)
                    .IsRequired();
            });

            entity.OwnsOne(p => p.Biography, bio =>
            {
                bio.Property(b => b.Content)
                    .HasColumnName("Biography")
                    .HasColumnType("TEXT");
            });

            entity.Property(p => p.ProfilePictureUrl)
                .HasMaxLength(500)
                .IsRequired(false);
        });

        // Si necesitas configuraciones adicionales:
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(DoctorDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Error al guardar cambios en la base de datos", ex);
        }
    }
}
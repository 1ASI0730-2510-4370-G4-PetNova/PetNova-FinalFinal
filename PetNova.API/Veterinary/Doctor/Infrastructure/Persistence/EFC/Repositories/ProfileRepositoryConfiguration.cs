namespace PetNova.API.Veterinary.Doctor.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;
public static class ProfileRepositoryConfiguration
{
    public static void ConfigureProfileAggregate(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoctorProfileAggregate>(entity =>
        {
            entity.ToTable("Profiles");
            entity.HasKey(p => p.Id);

            // Configuración de Value Objects como Owned Entities
            entity.OwnsOne(p => p.Name, name =>
            {
                name.Property(n => n.FirstName).HasColumnName("FirstName");
                name.Property(n => n.LastName).HasColumnName("LastName");
            });

            entity.OwnsOne(p => p.Specialty, specialty =>
            {
                specialty.Property(s => s.Value).HasColumnName("Specialty");
            });

            entity.OwnsOne(p => p.Biography, bio =>
            {
                bio.Property(b => b.Content).HasColumnName("Biography");
            });

            entity.Property(p => p.ProfilePictureUrl)
                .HasMaxLength(500);
        });
    }
}
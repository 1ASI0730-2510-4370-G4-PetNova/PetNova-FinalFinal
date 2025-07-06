using Microsoft.EntityFrameworkCore;
using PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;
using PetNova.API.Veterinary.IAM.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;

namespace PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<DoctorProfileAggregate> DoctorProfiles { get; set; }

    public DbSet<Pet> Pets { get; set; }
    public DbSet<User> Users { get; set; }
    public object Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Breed).IsRequired().HasMaxLength(100);
            entity.Property(p => p.DateOfBirth).IsRequired();
            entity.Property(p => p.DateRegistered).IsRequired();
            entity.Property(p => p.Gender).IsRequired();
            entity.Property(p => p.ClientId).IsRequired();
        });
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.OwnsOne(c => c.Name, name =>
            {
                name.Property(n => n.FirstName).IsRequired().HasMaxLength(50);
                name.Property(n => n.LastName).IsRequired().HasMaxLength(50);
            });

            entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Phone).IsRequired().HasMaxLength(20);
        });
        modelBuilder.Entity<DoctorProfileAggregate>(entity =>
        {
            entity.ToTable("profiles"); // Nombre que EF usará al crear la tabla

            entity.HasKey(p => p.Id);

            entity.OwnsOne(p => p.Name, name =>
            {
                name.Property(n => n.FirstName).HasColumnName("FirstName").HasMaxLength(50).IsRequired();
                name.Property(n => n.LastName).HasColumnName("LastName").HasMaxLength(50).IsRequired();
            });

            entity.OwnsOne(p => p.Specialty, specialty =>
            {
                specialty.Property(s => s.Value).HasColumnName("Specialty").HasMaxLength(100).IsRequired();
            });

            entity.OwnsOne(p => p.Biography, bio =>
            {
                bio.Property(b => b.Content).HasColumnName("Biography").HasMaxLength(500);
            });

            entity.Property(p => p.ProfilePictureUrl).HasMaxLength(200);
        });

    
   
    }
}
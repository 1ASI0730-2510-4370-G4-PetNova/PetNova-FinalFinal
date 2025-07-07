using Microsoft.EntityFrameworkCore;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregates;
using PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;
using PetNova.API.Veterinary.IAM.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;

namespace PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Appointment> Appointments { get; set; }

 
    public DbSet<Pet> Pets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }

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
            entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Phone).IsRequired().HasMaxLength(20);

            entity.OwnsOne(c => c.Name, name =>
            {
                name.Property(n => n.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(50);
                name.Property(n => n.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(50);
            });
        });
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.PetName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.ClientName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.ContactNumber).IsRequired().HasMaxLength(20);
            entity.Property(a => a.StartDate).IsRequired();

            entity.Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired();

            entity.Property(a => a.Type)
                .HasConversion<string>()
                .IsRequired();
        });



   
    }
}
using Microsoft.EntityFrameworkCore;
using PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;
using PetNova.API.Veterinary.IAM.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;

namespace PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

 
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
            entity.OwnsOne(c => c.Name, name =>
            {
                name.Property(n => n.FirstName).HasColumnName("FirstName").IsRequired();
                name.Property(n => n.LastName).HasColumnName("LastName").IsRequired();
            });

            entity.Property(c => c.Email).IsRequired();
            entity.Property(c => c.Phone).IsRequired();
        });

   
    }
}
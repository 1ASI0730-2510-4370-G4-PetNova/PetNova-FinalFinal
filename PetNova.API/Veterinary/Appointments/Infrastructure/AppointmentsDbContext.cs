using Microsoft.EntityFrameworkCore;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Pets.Domain.Model.Aggregate;

namespace PetNova.API.Veterinary.Appointments.Infrastructure;

public class AppointmentsDbContext : DbContext
{
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Client> Clients { get; set; }

    public AppointmentsDbContext(DbContextOptions<AppointmentsDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.StartDate)
                .IsRequired()
                .HasColumnType("datetime2");
                
            entity.Property(a => a.Duration)
                .IsRequired()
                .HasConversion(
                    v => v.Ticks,
                    v => TimeSpan.FromTicks(v));
            
            entity.Property(a => a.Status)
                .IsRequired()
                .HasConversion<string>();
                
            entity.Property(a => a.Type)
                .IsRequired()
                .HasConversion<string>();

            entity.HasOne(a => a.Pet)
                .WithMany()
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Client)
                .WithMany()
                .HasForeignKey(a => a.ClientId);

            entity.HasIndex(a => a.StartDate);
            entity.HasIndex(a => new { a.Status, a.StartDate });
        });

        modelBuilder.Entity<Pet>(entity => 
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(150);
            entity.Property(c => c.Phone).HasMaxLength(20);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}

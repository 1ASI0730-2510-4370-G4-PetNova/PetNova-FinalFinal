using Microsoft.AspNetCore.Identity;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PetNova.API.Veterinary.IAM.Domain.Model.Aggregate;

namespace PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
 
        // Seed admin user if none exists
        if (!context.Users.Any())
        {
            var passwordHasher = new PasswordHasher<User>();
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@petnova.com",
                Role = "Admin"
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");
            
            await context.Users.AddAsync(adminUser);
        }

        await context.SaveChangesAsync();
    }
}
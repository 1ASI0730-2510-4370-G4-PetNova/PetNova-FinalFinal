using Microsoft.EntityFrameworkCore;

namespace PetNova.API.Veterinary.Doctor.Infrastructure.Persistence.EFC;
using PetNova.API.Veterinary.Doctor.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Doctor.Domain.Repositories;
using Microsoft.Extensions.Logging;

public class ProfileRepository(
    DoctorDbContext context,
    ILogger<ProfileRepository> logger)
    : IProfileRepository
{
    public async Task<DoctorProfileAggregate?> GetByIdAsync(Guid id)
    {
        try
        {
            return await context.Profiles
                .Include(p => p.Name)
                .Include(p => p.Specialty)
                .Include(p => p.Biography)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving profile with ID {ProfileId}", id);
            throw;
        }
    }

    public async Task AddAsync(DoctorProfileAggregate profile)
    {
        try
        {
            await context.Profiles.AddAsync(profile);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding new profile");
            throw;
        }
    }

    public async Task UpdateAsync(DoctorProfileAggregate profile)
    {
        try
        {
            context.Profiles.Update(profile);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating profile with ID {ProfileId}", profile.Id);
            throw;
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var profile = await GetByIdAsync(id);
            if (profile != null)
            {
                context.Profiles.Remove(profile);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting profile with ID {ProfileId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Profiles.AnyAsync(p => p.Id == id);
    }
}
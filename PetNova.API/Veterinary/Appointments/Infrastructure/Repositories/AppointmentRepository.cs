using Microsoft.EntityFrameworkCore;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PetNova.API.Veterinary.Appointments.Domain.Model;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;
using PetNova.API.Veterinary.Appointments.Domain.Repositories;
using PetNova.API.Shared.Domain; // Añade esta línea (ajusta el namespace según donde tengas definido Entity<>)

namespace PetNova.API.Veterinary.Appointments.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var appointment = await GetByIdAsync(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Appointment?> GetByIdAsync(Guid id)
        => await _context.Appointments.FindAsync(id);

    public async Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status)
        => _context.Appointments.Where(a => a.Status == status);

    public async Task<IEnumerable<Appointment>> GetAllAsync(int page, int pageSize)
        => _context.Appointments.Skip((page - 1) * pageSize).Take(pageSize);
}


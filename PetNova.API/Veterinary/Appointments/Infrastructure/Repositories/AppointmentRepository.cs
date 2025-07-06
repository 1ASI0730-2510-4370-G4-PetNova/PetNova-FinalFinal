using Microsoft.EntityFrameworkCore;
using PetNova.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using PetNova.API.Veterinary.Appointments.Domain.Model.Aggregate;
using PetNova.API.Veterinary.Appointments.Domain.Model.ValueObjects;
using PetNova.API.Veterinary.Appointments.Domain.Repositories;
using PetNova.API.Shared.Domain; // Añade esta línea (ajusta el namespace según donde tengas definido Entity<>)

namespace PetNova.API.Veterinary.Appointments.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentsDbContext _context;

    public AppointmentRepository(AppointmentsDbContext context)
    {
        _context = context;
    }

    // Crear una nueva cita
    public async Task AddAsync(Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);
    }

    // Actualizar cita existente
    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Appointments.Update(appointment);
        await Task.CompletedTask;
    }

    // Eliminar cita (soft delete o físico)
    public async Task DeleteAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            _context.Appointments.Remove(appointment);
            await Task.CompletedTask;
        }
    }

    // Obtener cita por ID
    public async Task<Appointment?> GetByIdAsync(Guid id)
    {
        return await _context.Appointments
            .Include(a => a.Pet)
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    // Obtener citas por estado y fecha opcional
    public async Task<IEnumerable<Appointment>> GetByStatusAsync(
        AppointmentStatus status, 
        DateTime? fromDate = null)
    {
        var query = _context.Appointments
            .Where(a => a.Status == status);

        if (fromDate.HasValue)
            query = query.Where(a => a.StartDate >= fromDate.Value);

        return await query.ToListAsync();
    }

    // Obtener todas las citas (con paginación)
public async Task<IEnumerable<Appointment>> GetAllAsync(int page, int pageSize, string? sortBy)
{
    var query = _context.Appointments.AsQueryable();

    // Aplicar paginación
    return await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
}

    // Método adicional: Verificar solapamiento de horarios
    public async Task<Appointment?> GetByDateTimeAsync(DateTime startDate)
    {
        return await _context.Appointments
            .FirstOrDefaultAsync(a => a.StartDate == startDate);
    }
}
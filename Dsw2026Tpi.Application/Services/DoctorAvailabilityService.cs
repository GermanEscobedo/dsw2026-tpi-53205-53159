using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Dsw2026Tpi.Domain.Entities;
using Dsw2026Tpi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Tpi.Application.Services;

public class DoctorAvailabilityService : IDoctorAvailabilityService
{
    private readonly Dsw2026TpiDbContext _context;

    public DoctorAvailabilityService(Dsw2026TpiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorAvailabilityModel.Response>> GetAllAsync()
    {
        var availabilities = await _context.DoctorAvailabilities.ToListAsync();

        return availabilities.Select(a => new DoctorAvailabilityModel.Response(
            a.Id,
            a.DoctorId,
            a.DayOfWeek,
            a.StartTime,
            a.EndTime
        ));
    }

    public async Task<DoctorAvailabilityModel.Response?> GetByIdAsync(Guid id)
    {
        var availability = await _context.DoctorAvailabilities.FindAsync(id);
        if (availability == null) return null;

        return new DoctorAvailabilityModel.Response(
            availability.Id,
            availability.DoctorId,
            availability.DayOfWeek,
            availability.StartTime,
            availability.EndTime
        );
    }

    public async Task<IEnumerable<DoctorAvailabilityModel.Response>> GetByDoctorIdAsync(Guid doctorId)
    {
        var availabilities = await _context.DoctorAvailabilities
            .Where(a => a.DoctorId == doctorId)
            .ToListAsync();

        return availabilities.Select(a => new DoctorAvailabilityModel.Response(
            a.Id,
            a.DoctorId,
            a.DayOfWeek,
            a.StartTime,
            a.EndTime
        ));
    }

    public async Task<DoctorAvailabilityModel.Response> CreateAsync(DoctorAvailabilityModel.Request model)
    {
        
        var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == model.DoctorId);
        if (!doctorExists) throw new Exception("El médico especificado no existe.");

        var availability = new DoctorAvailability(model.DoctorId, model.DayOfWeek, model.StartTime, model.EndTime);

        await _context.DoctorAvailabilities.AddAsync(availability);
        await _context.SaveChangesAsync();

        return new DoctorAvailabilityModel.Response(
            availability.Id,
            availability.DoctorId,
            availability.DayOfWeek,
            availability.StartTime,
            availability.EndTime
        );
    }

    public async Task UpdateAsync(Guid id, DoctorAvailabilityModel.Request model)
    {
        var availability = await _context.DoctorAvailabilities.FindAsync(id);
        if (availability == null) throw new Exception("Disponibilidad no encontrada.");

        availability.Update(model.DayOfWeek, model.StartTime, model.EndTime);

        _context.DoctorAvailabilities.Update(availability);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var availability = await _context.DoctorAvailabilities.FindAsync(id);
        if (availability == null) throw new Exception("Disponibilidad no encontrada.");

        _context.DoctorAvailabilities.Remove(availability);
        await _context.SaveChangesAsync();
    }
}
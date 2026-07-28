using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Dsw2026Tpi.Domain.Entities;
using Dsw2026Tpi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Tpi.Application.Services;

public class SpecialityService : ISpecialityService
{
    private readonly Dsw2026TpiDbContext _context;

    public SpecialityService(Dsw2026TpiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SpecialityModel.Response>> GetAllAsync()
    {
        var specialities = await _context.Specialities.ToListAsync();

        return specialities.Select(s => new SpecialityModel.Response(s.Id, s.Name, s.Description));
    }

    public async Task<SpecialityModel.Response?> GetByIdAsync(Guid id)
    {
        var speciality = await _context.Specialities.FindAsync(id);
        if (speciality == null) return null;

        return new SpecialityModel.Response(speciality.Id, speciality.Name, speciality.Description);
    }

    public async Task<SpecialityModel.Response> CreateAsync(SpecialityModel.Request model)
    {
        // Creamos la nueva especialidad instanciando la entidad de Dominio
        var speciality = new Speciality(model.Name, model.Description);

        await _context.Specialities.AddAsync(speciality);
        await _context.SaveChangesAsync();

        return new SpecialityModel.Response(speciality.Id, speciality.Name, speciality.Description);
    }

    public async Task UpdateAsync(Guid id, SpecialityModel.Request model)
    {
        var speciality = await _context.Specialities.FindAsync(id);
        if (speciality == null) throw new Exception("Especialidad no encontrada");

        // Usamos el método de dominio que acabamos de asegurar en el Paso 1
        speciality.Update(model.Name, model.Description);

        _context.Specialities.Update(speciality);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var speciality = await _context.Specialities.FindAsync(id);
        if (speciality == null) throw new Exception("Especialidad no encontrada");

        _context.Specialities.Remove(speciality);
        await _context.SaveChangesAsync();
    }
}
using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Dsw2026Tpi.Domain.Entities;
using Dsw2026Tpi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Tpi.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly Dsw2026TpiDbContext _context;

    public DoctorService(Dsw2026TpiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorModel.Response>> GetAllAsync()
    {
        var doctors = await _context.Doctors
            .Include(d => d.Speciality) 
            .ToListAsync();

        return doctors.Select(d => new DoctorModel.Response(
            d.Id,
            d.FirstName,
            d.LastName,
            d.Email,
            d.LicenseNumber,
            new DoctorModel.SpecialityDto(d.Speciality?.Id, d.Speciality?.Name)
        ));
    }

    public async Task<DoctorModel.Response?> GetByIdAsync(Guid id)
    {
        var doctor = await _context.Doctors
            .Include(d => d.Speciality)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (doctor == null) return null;

        return new DoctorModel.Response(
            doctor.Id,
            doctor.FirstName,
            doctor.LastName,
            doctor.Email,
            doctor.LicenseNumber,
            new DoctorModel.SpecialityDto(doctor.Speciality?.Id, doctor.Speciality?.Name)
        );
    }

    public async Task<DoctorModel.Response> CreateAsync(DoctorModel.Request model)
    {
        var doctor = new Doctor(model.FirstName, model.LastName, model.Email, model.LicenseNumber, model.SpecialityId);

        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();

        // Recargamos la relación para traer la especialidad completa en la respuesta
        await _context.Entry(doctor).Reference(d => d.Speciality).LoadAsync();

        return new DoctorModel.Response(
            doctor.Id,
            doctor.FirstName,
            doctor.LastName,
            doctor.Email,
            doctor.LicenseNumber,
            new DoctorModel.SpecialityDto(doctor.Speciality?.Id, doctor.Speciality?.Name)
        );
    }

    public async Task UpdateAsync(Guid id, DoctorModel.Request model)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) throw new Exception("Médico no encontrado");

        doctor.Update(model.FirstName, model.LastName, model.Email, model.LicenseNumber, model.SpecialityId);

        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) throw new Exception("Médico no encontrado");

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync();
    }
}
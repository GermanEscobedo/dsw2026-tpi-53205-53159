using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Dsw2026Tpi.Domain.Entities;
using Dsw2026Tpi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Tpi.Application.Services;

public class PatientService : IPatientService
{
    private readonly Dsw2026TpiDbContext _context;

    // Inyectamos el DbContext para poder acceder a la base de datos
    public PatientService(Dsw2026TpiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientModel.Response>> GetAllAsync()
    {
        var patients = await _context.Patients.ToListAsync();

        // Mapeamos de Entidad a DTO de Respuesta
        return patients.Select(p => new PatientModel.Response(p.Id, p.FirstName, p.LastName, p.Email, p.Dni, p.PhoneNumber));
    }

    public async Task<PatientModel.Response?> GetByIdAsync(Guid id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return null;

        return new PatientModel.Response(patient.Id, patient.FirstName, patient.LastName, patient.Email, patient.Dni, patient.PhoneNumber);
    }

    public async Task<PatientModel.Response> CreateAsync(PatientModel.Request model)
    {
        // Transformamos el DTO de entrada en una Entidad de Dominio real
        var patient = new Patient(model.FirstName, model.LastName, model.Email, model.Dni, model.PhoneNumber);

        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Devolvemos el paciente ya creado (ahora con su ID generado)
        return new PatientModel.Response(patient.Id, patient.FirstName, patient.LastName, patient.Email, patient.Dni, patient.PhoneNumber);
    }

    public async Task UpdateAsync(Guid id, PatientModel.Request model)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) throw new Exception("Paciente no encontrado");

        // Usamos el método de negocio de la entidad para actualizar los datos
        patient.Update(
            model.FirstName,
            model.LastName,
            model.Email,
            model.Dni,
            model.PhoneNumber
        );

        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) throw new Exception("Paciente no encontrado");

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
    }
}

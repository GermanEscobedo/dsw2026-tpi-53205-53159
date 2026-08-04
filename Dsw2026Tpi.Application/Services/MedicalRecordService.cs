using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Dsw2026Tpi.Domain.Entities;
using Dsw2026Tpi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Tpi.Application.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly Dsw2026TpiDbContext _context;

    public MedicalRecordService(Dsw2026TpiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MedicalRecordModel.Response>> GetAllAsync()
    {
        var records = await _context.MedicalRecords.ToListAsync();

        return records.Select(r => new MedicalRecordModel.Response(
            r.Id,
            r.PatientId,
            r.DoctorId,
            r.AppointmentId,
            r.ConsultationDate,
            r.Diagnosis,
            r.Treatment,
            r.Observations
        ));
    }

    public async Task<MedicalRecordModel.Response?> GetByIdAsync(Guid id)
    {
        var record = await _context.MedicalRecords.FindAsync(id);
        if (record == null) return null;

        return new MedicalRecordModel.Response(
            record.Id,
            record.PatientId,
            record.DoctorId,
            record.AppointmentId,
            record.ConsultationDate,
            record.Diagnosis,
            record.Treatment,
            record.Observations
        );
    }

    public async Task<IEnumerable<MedicalRecordModel.Response>> GetByPatientIdAsync(Guid patientId)
    {
        var records = await _context.MedicalRecords
            .Where(r => r.PatientId == patientId)
            .ToListAsync();

        return records.Select(r => new MedicalRecordModel.Response(
            r.Id,
            r.PatientId,
            r.DoctorId,
            r.AppointmentId,
            r.ConsultationDate,
            r.Diagnosis,
            r.Treatment,
            r.Observations
        ));
    }

    public async Task<IEnumerable<MedicalRecordModel.Response>> GetByDoctorIdAsync(Guid doctorId)
    {
        var records = await _context.MedicalRecords
            .Where(r => r.DoctorId == doctorId)
            .ToListAsync();

        return records.Select(r => new MedicalRecordModel.Response(
            r.Id,
            r.PatientId,
            r.DoctorId,
            r.AppointmentId,
            r.ConsultationDate,
            r.Diagnosis,
            r.Treatment,
            r.Observations
        ));
    }

    public async Task<MedicalRecordModel.Response> CreateAsync(MedicalRecordModel.Request model)
    {
        var patientExists = await _context.Patients.AnyAsync(p => p.Id == model.PatientId);
        if (!patientExists) throw new Exception("El paciente especificado no existe.");

        var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == model.DoctorId);
        if (!doctorExists) throw new Exception("El médico especificado no existe.");

        if (model.AppointmentId.HasValue)
        {
            var appointmentExists = await _context.Appointments.AnyAsync(a => a.Id == model.AppointmentId.Value);
            if (!appointmentExists) throw new Exception("El turno especificado no existe.");
        }

        var medicalRecord = new MedicalRecord(
            model.PatientId,
            model.DoctorId,
            model.AppointmentId,
            model.ConsultationDate,
            model.Diagnosis,
            model.Treatment,
            model.Observations
        );

        await _context.MedicalRecords.AddAsync(medicalRecord);
        await _context.SaveChangesAsync();

        return new MedicalRecordModel.Response(
            medicalRecord.Id,
            medicalRecord.PatientId,
            medicalRecord.DoctorId,
            medicalRecord.AppointmentId,
            medicalRecord.ConsultationDate,
            medicalRecord.Diagnosis,
            medicalRecord.Treatment,
            medicalRecord.Observations
        );
    }

    public async Task UpdateAsync(Guid id, MedicalRecordModel.Request model)
    {
        var record = await _context.MedicalRecords.FindAsync(id);
        if (record == null) throw new Exception("Historia clínica no encontrada.");

        record.Update(model.Diagnosis, model.Treatment, model.Observations);

        _context.MedicalRecords.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var record = await _context.MedicalRecords.FindAsync(id);
        if (record == null) throw new Exception("Historia clínica no encontrada.");

        _context.MedicalRecords.Remove(record);
        await _context.SaveChangesAsync();
    }
}
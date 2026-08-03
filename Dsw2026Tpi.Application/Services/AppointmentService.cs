using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Dsw2026Tpi.Domain.Entities;
using Dsw2026Tpi.Data;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Tpi.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly Dsw2026TpiDbContext _context;

    public AppointmentService(Dsw2026TpiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppointmentModel.Response>> GetAllAsync()
    {
        var appointments = await _context.Appointments.ToListAsync();

        return appointments.Select(a => new AppointmentModel.Response(
            a.Id,
            a.PatientId,
            a.DoctorId,
            a.AppointmentDateTime,
            a.Reason,
            a.IsCancelled
        ));
    }

    public async Task<AppointmentModel.Response?> GetByIdAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) return null;

        return new AppointmentModel.Response(
            appointment.Id,
            appointment.PatientId,
            appointment.DoctorId,
            appointment.AppointmentDateTime,
            appointment.Reason,
            appointment.IsCancelled
        );
    }

    public async Task<IEnumerable<AppointmentModel.Response>> GetByPatientIdAsync(Guid patientId)
    {
        var appointments = await _context.Appointments
            .Where(a => a.PatientId == patientId)
            .ToListAsync();

        return appointments.Select(a => new AppointmentModel.Response(
            a.Id,
            a.PatientId,
            a.DoctorId,
            a.AppointmentDateTime,
            a.Reason,
            a.IsCancelled
        ));
    }

    public async Task<IEnumerable<AppointmentModel.Response>> GetByDoctorIdAsync(Guid doctorId)
    {
        var appointments = await _context.Appointments
            .Where(a => a.DoctorId == doctorId)
            .ToListAsync();

        return appointments.Select(a => new AppointmentModel.Response(
            a.Id,
            a.PatientId,
            a.DoctorId,
            a.AppointmentDateTime,
            a.Reason,
            a.IsCancelled
        ));
    }

    public async Task<AppointmentModel.Response> CreateAsync(AppointmentModel.Request model)
    {
        var patientExists = await _context.Patients.AnyAsync(p => p.Id == model.PatientId);
        if (!patientExists) throw new Exception("El paciente especificado no existe.");

        var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == model.DoctorId);
        if (!doctorExists) throw new Exception("El médico especificado no existe.");

        var dayOfWeek = model.AppointmentDateTime.DayOfWeek;
        var appointmentTime = model.AppointmentDateTime.TimeOfDay;

        var hasAvailability = await _context.DoctorAvailabilities.AnyAsync(a =>
            a.DoctorId == model.DoctorId &&
            a.DayOfWeek == dayOfWeek &&
            appointmentTime >= a.StartTime &&
            appointmentTime <= a.EndTime
        );

        if (!hasAvailability)
        {
            throw new Exception("El médico no cuenta con disponibilidad horaria en el día y horario seleccionado.");
        }

        var conflictiveAppointment = await _context.Appointments.AnyAsync(a =>
            a.DoctorId == model.DoctorId &&
            a.AppointmentDateTime == model.AppointmentDateTime &&
            !a.IsCancelled
        );

        if (conflictiveAppointment)
        {
            throw new Exception("El médico ya tiene un turno reservado en ese mismo horario.");
        }

        var appointment = new Appointment(model.PatientId, model.DoctorId, model.AppointmentDateTime, model.Reason);

        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        return new AppointmentModel.Response(
            appointment.Id,
            appointment.PatientId,
            appointment.DoctorId,
            appointment.AppointmentDateTime,
            appointment.Reason,
            appointment.IsCancelled
        );
    }

    public async Task UpdateAsync(Guid id, AppointmentModel.Request model)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) throw new Exception("Turno no encontrado.");

        var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == model.DoctorId);
        if (!doctorExists) throw new Exception("El médico especificado no existe.");

        var dayOfWeek = model.AppointmentDateTime.DayOfWeek;
        var appointmentTime = model.AppointmentDateTime.TimeOfDay;

        var hasAvailability = await _context.DoctorAvailabilities.AnyAsync(a =>
            a.DoctorId == model.DoctorId &&
            a.DayOfWeek == dayOfWeek &&
            appointmentTime >= a.StartTime &&
            appointmentTime <= a.EndTime
        );

        if (!hasAvailability)
        {
            throw new Exception("El médico no cuenta con disponibilidad horaria en el día y horario seleccionado.");
        }

        var conflictiveAppointment = await _context.Appointments.AnyAsync(a =>
            a.Id != id &&
            a.DoctorId == model.DoctorId &&
            a.AppointmentDateTime == model.AppointmentDateTime &&
            !a.IsCancelled
        );

        if (conflictiveAppointment)
        {
            throw new Exception("El médico ya tiene un turno reservado en ese mismo horario.");
        }

        appointment.Update(model.AppointmentDateTime, model.Reason);

        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task CancelAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) throw new Exception("Turno no encontrado.");

        appointment.Cancel();

        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null) throw new Exception("Turno no encontrado.");

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task<object> SearchAsync(int pageSize, int pageIndex)
    {
        var total = await _context.Appointments.CountAsync();
        var data = await _context.Appointments
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new { pageSize, pageIndex, data, total };
    }

    public async Task<AppointmentModel.SearchResponse> SearchAsync(int pageSize, int pageIndex, Guid? specialtyId, Guid? doctorId, string? dni, DateTime? date)
    {
        var query = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
                .ThenInclude(d => d!.Speciality)
            .AsQueryable();

        if (specialtyId.HasValue)
            query = query.Where(a => a.Doctor!.SpecialityId == specialtyId.Value);

        if (doctorId.HasValue)
            query = query.Where(a => a.DoctorId == doctorId.Value);

        if (!string.IsNullOrWhiteSpace(dni))
            query = query.Where(a => a.Patient!.Dni == dni);

        if (date.HasValue)
            query = query.Where(a => a.AppointmentDateTime.Date == date.Value.Date);

        var total = await query.CountAsync();

        var appointments = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var data = appointments.Select(a => new AppointmentModel.SearchItemResponse(
             a.Id,
             a.IsCancelled ? "CANCELLED" : "BOOKED",
             new AppointmentModel.PatientDetailResponse(a.Patient!.Dni, $"{a.Patient.FirstName} {a.Patient.LastName}"),
             new AppointmentModel.DoctorDetailResponse(
                 a.Doctor!.Id,
                 $"{a.Doctor.FirstName} {a.Doctor.LastName}",
                 new AppointmentModel.SpecialtyDetailResponse(
                     a.Doctor.Speciality!.Id,
                     a.Doctor.Speciality!.Name
                 )
             )
         ));

        return new AppointmentModel.SearchResponse(pageSize, pageIndex, data, total);
    }

}   
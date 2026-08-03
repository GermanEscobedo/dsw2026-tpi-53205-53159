using Dsw2026Tpi.Application.Dtos;

namespace Dsw2026Tpi.Application.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentModel.Response>> GetAllAsync();
    Task<AppointmentModel.Response?> GetByIdAsync(Guid id);
    Task<IEnumerable<AppointmentModel.Response>> GetByPatientIdAsync(Guid patientId);
    Task<IEnumerable<AppointmentModel.Response>> GetByDoctorIdAsync(Guid doctorId);
    Task<AppointmentModel.Response> CreateAsync(AppointmentModel.Request model);
    Task UpdateAsync(Guid id, AppointmentModel.Request model);
    Task CancelAsync(Guid id);
    Task DeleteAsync(Guid id);
}
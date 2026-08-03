using Dsw2026Tpi.Application.Dtos;

namespace Dsw2026Tpi.Application.Interfaces;

public interface IDoctorAvailabilityService
{
    Task<IEnumerable<DoctorAvailabilityModel.Response>> GetAllAsync();
    Task<DoctorAvailabilityModel.Response?> GetByIdAsync(Guid id);
    Task<IEnumerable<DoctorAvailabilityModel.Response>> GetByDoctorIdAsync(Guid doctorId);
    Task<DoctorAvailabilityModel.Response> CreateAsync(DoctorAvailabilityModel.Request model);
    Task UpdateAsync(Guid id, DoctorAvailabilityModel.Request model);
    Task DeleteAsync(Guid id);
}
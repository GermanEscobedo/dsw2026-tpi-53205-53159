using Dsw2026Tpi.Application.Dtos;

namespace Dsw2026Tpi.Application.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorModel.Response>> GetAllAsync();
    Task<DoctorModel.Response?> GetByIdAsync(Guid id);
    Task<DoctorModel.Response> CreateAsync(DoctorModel.Request model);
    Task UpdateAsync(Guid id, DoctorModel.Request model);
    Task DeleteAsync(Guid id);
}
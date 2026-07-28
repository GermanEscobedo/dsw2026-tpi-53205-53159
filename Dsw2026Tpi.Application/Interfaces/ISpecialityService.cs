using Dsw2026Tpi.Application.Dtos;

namespace Dsw2026Tpi.Application.Interfaces;

public interface ISpecialityService
{
    Task<IEnumerable<SpecialityModel.Response>> GetAllAsync();
    Task<SpecialityModel.Response?> GetByIdAsync(Guid id);
    Task<SpecialityModel.Response> CreateAsync(SpecialityModel.Request model);
    Task UpdateAsync(Guid id, SpecialityModel.Request model);
    Task DeleteAsync(Guid id); // Borrado lógico
}
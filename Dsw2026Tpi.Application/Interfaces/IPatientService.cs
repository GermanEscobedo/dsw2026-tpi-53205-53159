using Dsw2026Tpi.Application.Dtos;

namespace Dsw2026Tpi.Application.Interfaces;

public interface IPatientService
{
    // Lista Completa
    Task<IEnumerable<PatientModel.Response>> GetAllAsync();

    // Busca 
    Task<PatientModel.Response?> GetByIdAsync(Guid id);

    // Crea 
    Task<PatientModel.Response> CreateAsync(PatientModel.Request model);

    // Actualiza 
    Task UpdateAsync(Guid id, PatientModel.Request model);

    // Elimina 
    Task DeleteAsync(Guid id);
}
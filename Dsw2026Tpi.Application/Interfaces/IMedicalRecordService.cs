using Dsw2026Tpi.Application.Dtos;

namespace Dsw2026Tpi.Application.Interfaces;

public interface IMedicalRecordService
{
    Task<IEnumerable<MedicalRecordModel.Response>> GetAllAsync();
    Task<MedicalRecordModel.Response?> GetByIdAsync(Guid id);
    Task<IEnumerable<MedicalRecordModel.Response>> GetByPatientIdAsync(Guid patientId);
    Task<IEnumerable<MedicalRecordModel.Response>> GetByDoctorIdAsync(Guid doctorId);
    Task<MedicalRecordModel.Response> CreateAsync(MedicalRecordModel.Request model);
    Task UpdateAsync(Guid id, MedicalRecordModel.Request model);
    Task DeleteAsync(Guid id);
}
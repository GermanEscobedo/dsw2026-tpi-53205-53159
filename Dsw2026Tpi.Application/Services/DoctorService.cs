using Dsw2026Tpi.Application.Dtos;
using Dsw2026Tpi.Application.Interfaces;
using Dsw2026Tpi.Domain.Entities;
using Dsw2026Tpi.Domain.Interfaces;

namespace Dsw2026Tpi.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IPersistence _persistence;

    public DoctorService(IPersistence persistence)
    {
        _persistence = persistence;
    }

    public async Task<Pagination<DoctorModel.Response>> GetAll(int pageSize, int pageIndex, string? name = null)
    {
        var doctors = await _persistence.Paginate<Doctor, string>(
            pageSize,
            pageIndex,
            // 1. Corregimos el filtro: Buscamos si el texto coincide con el Nombre O el Apellido
            d => string.IsNullOrWhiteSpace(name) ||
                 d.FirstName.Contains(name) ||
                 d.LastName.Contains(name),
            // 2. Corregimos el ordenamiento: Los ordenamos por Apellido
            x => x.LastName,
            nameof(Doctor.Speciality)
        );

        // 3. Corregimos el mapeo de los datos
        return doctors.Map(d => new DoctorModel.Response(
            d.Id,
            $"{d.FirstName} {d.LastName}", // Unimos Nombre y Apellido (si tu DTO espera un solo 'Name')
            d.LicenseNumber,
            new DoctorModel.SpecialityDto(d.Speciality?.Id, d.Speciality?.Name)
        ));
    }
}

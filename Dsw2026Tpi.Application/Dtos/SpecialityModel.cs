namespace Dsw2026Tpi.Application.Dtos;

public class SpecialityModel
{
    // Datos que pedimos para crear o actualizar una especialidad
    public record Request(string Name, string Description);

    // Datos que devolvemos al consultar
    public record Response(Guid Id, string Name, string Description);
}
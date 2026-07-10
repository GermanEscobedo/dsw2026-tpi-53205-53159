namespace Dsw2026Tpi.Application.Dtos;

public record PatientModel
{
    public record Request(string FirstName, string LastName, string Email, string Dni, string PhoneNumber);
    public record Response(Guid Id, string FirstName, string LastName, string Email, string Dni, string PhoneNumber);
}
namespace Dsw2026Tpi.Application.Dtos;

public class DoctorModel
{
  
    public record SpecialityDto(Guid? Id, string? Name);

 
    public record Request( string FirstName, string LastName, string Email, string LicenseNumber, Guid SpecialityId);

    
    public record Response( Guid Id, string FirstName, string LastName, string Email, string LicenseNumber, SpecialityDto? Speciality);
}
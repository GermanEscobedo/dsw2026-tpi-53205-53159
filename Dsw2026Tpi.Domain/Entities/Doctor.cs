namespace Dsw2026Tpi.Domain.Entities;

public class Doctor : EntityBase
{
   
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string LicenseNumber { get; private set; }

    public Guid SpecialityId { get; private set; }

    // Propiedad de navegación (Le dice a EF Core que busque la entidad real)
    public Speciality? Speciality { get; private set; }

    public Doctor(string firstName, string lastName, string email, string licenseNumber, Guid specialityId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        LicenseNumber = licenseNumber;
        SpecialityId = specialityId;
    }

    public void Update(string firstName, string lastName, string email, string licenseNumber, Guid specialityId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        LicenseNumber = licenseNumber;
        SpecialityId = specialityId;
    }
}
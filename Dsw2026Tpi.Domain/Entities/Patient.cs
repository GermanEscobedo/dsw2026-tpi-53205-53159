using Dsw2026Tpi.Domain.Entities;

namespace Dsw2026Tpi.Domain.Entities;

public class Patient : EntityBase
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Dni { get; private set; }
    public string PhoneNumber { get; private set; }
    public bool IsActive { get; private set; }

    #region Constructor for EF
#pragma warning disable CS8618
    private Patient() { }
#pragma warning restore CS8618
    #endregion

    public Patient(string firstName, string lastName, string email, string dni, string phoneNumber, Guid? id = null) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Dni = dni;
        PhoneNumber = phoneNumber;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Update(string firstName, string lastName, string email, string dni, string phoneNumber)
    {
        // Aquí la entidad sí tiene permiso para modificarse a sí misma
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Dni = dni;
        PhoneNumber = phoneNumber;
    }

}   
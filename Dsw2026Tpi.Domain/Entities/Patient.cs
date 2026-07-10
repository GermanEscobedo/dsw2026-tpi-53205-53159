using Dsw2026Tpi.Domain.Entities;

namespace Dsw2026Tpi.Domain.Entities;

public class Patient : EntityBase
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Dni { get; init; }
    public string PhoneNumber { get; init; }
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
}
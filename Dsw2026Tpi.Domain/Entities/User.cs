namespace Dsw2026Tpi.Domain.Entities;

public class User : EntityBase
{
    public string Email { get; private set; } = string.Empty;
    public string? PasswordHash { get; private set; }
    public string Role { get; private set; } = string.Empty; 

    public User(string email, string? passwordHash, string role)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}
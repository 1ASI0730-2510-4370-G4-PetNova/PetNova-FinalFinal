using PetNova.API.Veterinary.Clients.Domain.Model.ValueObjects;
using System; // Required for Guid

namespace PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;

public partial class Client // Assuming Entity<Guid> base or similar for Id, CreatedAt, UpdatedAt if following patterns from Appointment
{
    public Guid Id { get; private set; }
    public FullName Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public string Address { get; private set; } // Added Address
    public string PasswordHash { get; private set; } // Added PasswordHash
    public DateTime CreatedAt { get; private set; } // Common practice
    public DateTime? UpdatedAt { get; private set; } // Common practice


    // Parameterless constructor for EF Core or serializers
    private Client() { }

    // Constructor for creating a new client
    public Client(FullName name, string email, string phone, string address, string passwordHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    // Overload for existing ClientCommandService which doesn't pass passwordHash or address
    public Client(string name, string email, string phone, string address, string passwordHash)
    {
        Id = Guid.NewGuid();
        // This assumes 'name' is a simple string, not FullName. Adjust if FullName is mandatory from start.
        // For now, splitting by space for FirstName, LastName as a simple approach.
        var nameParts = name.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
        Name = new FullName(nameParts.Length > 0 ? nameParts[0] : "", nameParts.Length > 1 ? nameParts[1] : "");
        Email = email;
        Phone = phone;
        Address = address; // Added
        PasswordHash = passwordHash; // Added
        CreatedAt = DateTime.UtcNow;
    }


    public void Update(FullName name, string phone, string email, string address)
    {
        Name = name;
        Phone = phone;
        Email = email; // Added Email update
        Address = address; // Added Address update
        UpdatedAt = DateTime.UtcNow;
    }

    // Overload to match existing simple Update in ClientCommandService
     public void Update(string name, string phone)
    {
        var nameParts = name.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
        Name = new FullName(nameParts.Length > 0 ? nameParts[0] : "", nameParts.Length > 1 ? nameParts[1] : "");
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }


    public void UpdateEmail(string email)
    {
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateAddress(string address)
    {
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }
}


using PetNova.API.Veterinary.Clients.Domain.Model.ValueObjects;
using PetNova.API.Veterinary.Clients.Domain.Model.ValueObjects;

namespace PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;
/// <summary>
///     Represents a client entity in Petnova.
/// </summary>
/// <summary>
///     Represents a client entity in Petnova.
/// </summary>
public partial class Client
{
    public Guid Id { get; private set; }
    public FullName Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    
    public Client() { }

    public Client(FullName name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }

    public void Update(FullName name, string phone)
    {
        Name = name;
        Phone = phone;
    }
}


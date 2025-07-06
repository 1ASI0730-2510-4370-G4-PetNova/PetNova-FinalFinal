namespace PetNova.API.Veterinary.Clients.Interfaces.REST.Resources;

public class ClientResource
{
    public Guid Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Phone { get; }

    public ClientResource(Guid id, string firstName, string lastName, string email, string phone)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }
}

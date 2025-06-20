namespace PetNova.API.Veterinary.ClientAndPetManagement.Interface.DTOs;

public class PetDTO
{
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Color { get; set; }
    public string Gender { get; set; }
    public string MicrochipId { get; set; }
    public Guid ClientId { get; set; }
}
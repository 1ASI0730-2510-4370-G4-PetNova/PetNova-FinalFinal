namespace PetNova.API.Veterinary.Status.Interface.DTOs;

public class StatusDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public bool IsActive { get; set; } = true;
}
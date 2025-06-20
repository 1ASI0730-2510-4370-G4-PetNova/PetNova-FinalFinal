using System.Security.Claims;
using PetNova.API.Veterinary.IAM.Domain.Model.Aggregate;

namespace PetNova.API.Shared.Application.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
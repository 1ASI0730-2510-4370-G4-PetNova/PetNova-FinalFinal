using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;

namespace PetNova.API.Veterinary.Clients.Domain.Model.Aggregate;

public partial class Client: IEntityWithCreatedUpdatedDate{
[Column("CreatedAt")] public DateTimeOffset? CreatedDate { get; set; }
[Column("UpdatedAt")] public DateTimeOffset? UpdatedDate { get; set; }
}
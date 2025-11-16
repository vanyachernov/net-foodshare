using Foodshare.Core.Common;

namespace Foodshare.Core.Entities;

public class Reservation : BaseAuditableEntity
{
    public Guid DishId { get; set; }
    public Dish Dish { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
using Foodshare.Core.Common;
using Foodshare.Core.Enums;

namespace Foodshare.Core.Entities;

public class Dish : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; } // 0 if free
    public DishCategory Category { get; set; }
    public string PhotoUrl { get; set; } = null!;
    public bool IsAvailable { get; set; } = true;
    public int? QuantityAvailable { get; set; }

    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public List<Reservation> Reservations { get; set; } = new();
}
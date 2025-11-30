using Foodshare.Core.Common;
using Foodshare.Core.Enums;

namespace Foodshare.Core.Entities;

public class Reservation : BaseAuditableEntity
{
    public Guid DishId { get; set; }
    public Dish Dish { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public string? Notes { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? PickupTime { get; set; }
}
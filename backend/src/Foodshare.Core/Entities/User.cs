using Foodshare.Core.Common;

namespace Foodshare.Core.Entities;

public class User : BaseAuditableEntity
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public List<Dish> Dishes { get; set; } = new();
    public List<Reservation> Reservations { get; set; } = new();
}
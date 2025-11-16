using Foodshare.Core.Common;

namespace Foodshare.Core.Entities;

public class Dish : BaseAuditableEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; } // 0 if free
    public string Category { get; set; } = null!;
    public string PhotoUrl { get; set; } = null!;

    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public List<Reservation> Reservations { get; set; } = new();
}
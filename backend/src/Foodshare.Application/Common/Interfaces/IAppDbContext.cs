using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Common.Interfaces;

public interface IAppDbContext
{
    // DbSet<User> Users { get; }
    // DbSet<Profile> Profiles { get; }
    // DbSet<Listing> Listings { get; }
    // DbSet<Booking> Bookings { get; }
    // DbSet<Media> Media { get; }
    // DbSet<Category> Categories { get; }
    // DbSet<Tag> Tags { get; }
    // DbSet<Review> Reviews { get; }
    // DbSet<PaymentTransaction> PaymentTransactions { get; }
    // DbSet<Report> Reports { get; }
    // DbSet<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}